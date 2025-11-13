using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ConsumptionManager : IConsumptionManager
    {
        private readonly string _compId;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly IBuyOrdShipDetailRepository _buyOrdShipDetailRepository;
        private readonly IConsumptionDetailRepository _consumptionDetail;
        private readonly ICompConsumptionRepository _compConsumptionRepository;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        public ConsumptionManager(IConsumptionRepository consumptionRepository, 
            IBuyOrdShipDetailRepository buyOrdShipDetailRepository, 
            ICompConsumptionRepository compConsumptionRepository,
            IConsumptionDetailRepository consumptionDetail,
            IUserMerchandiserRepository userMerchandiserRepository)
        {
            _buyOrdShipDetailRepository = buyOrdShipDetailRepository;
            _compId = PortalContext.CurrentUser.CompId;
            _consumptionRepository = consumptionRepository;
            _consumptionDetail = consumptionDetail;
            _compConsumptionRepository = compConsumptionRepository;
            _userMerchandiserRepository = userMerchandiserRepository;
        }

        public List<VConsumption> GetConsumptions(string orderStyleRefId, string consGroup)
        {
            return _consumptionRepository.GetConsumptions(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId && x.ConsGroup == consGroup).ToList();
        }
        public int SaveConsumption(OM_Consumption model)
        {
            var saveIndex = 0;
            bool isExist =_consumptionRepository.Exists(x => x.CompId == _compId && x.OrderStyleRefId == model.OrderStyleRefId  && x.ItemCode == model.ItemCode);
            if (isExist)
            {
                throw new Exception("Item already Added");
            }
            else
            {
                using (var transaction = new TransactionScope())
                {
                    model.ConsRefId = GetNewConsRefId();
                    model.CompId = _compId;
                    saveIndex = _consumptionRepository.Save(model);
                    saveIndex += UpdateConsuptionDetails(model);
                    transaction.Complete();
                }
 
            }
        
            return saveIndex;

        }
        public string GetNewConsRefId()
        {
            return _consumptionRepository.GetNewConsRefId(_compId);
        }
        public VConsumption GetVConsumptionById(long consumptionId)
        {
            return _consumptionRepository.GetConsumptions(x => x.CompId == _compId && x.ConsumptionId == consumptionId).FirstOrDefault();
        }
        public int EditConsumption(OM_Consumption model)
        {
            var saveIndex = 0;
            var consumption = _consumptionRepository.FindOne(x => x.ConsumptionId == model.ConsumptionId);
            consumption.OrderStyleRefId = model.OrderStyleRefId;
            consumption.ConsRefId = model.ConsRefId;
            consumption.ItemCode = model.ItemCode;
            consumption.Quantity = model.Quantity;
            consumption.ConsTypeRefId = model.ConsTypeRefId;
            consumption.ConsGroup = model.ConsGroup;
            consumption.ConsDate = model.ConsDate;
            consumption.ItemDescription = model.ItemDescription;
            saveIndex = _consumptionDetail.Delete(x => x.ConsRefId == model.ConsRefId && x.CompId == _compId);
            saveIndex += _consumptionRepository.Edit(consumption);
            model.CompId = _compId;
            saveIndex += UpdateConsuptionDetails(model);
            return saveIndex;
        }

        public List<VConsumption> GetConsumptionsFabric(string orderStyleRefId)
        {
            return _consumptionRepository.GetConsumptions(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId && x.ItemCode.StartsWith("10")&&x.ConsGroup=="P").ToList();
        }

        public int UpdateConstumptionCost(List<OM_Consumption> consumptions)
        {
            var updateIndex = 0;

            foreach (var consumption in consumptions)
            {
                var consump = _consumptionRepository.FindOne(x => x.ConsumptionId == consumption.ConsumptionId && x.CompId == _compId && x.OrderStyleRefId == consumption.OrderStyleRefId);
                consump.Rate = consumption.Rate;
                consump.SupplierId = consumption.SupplierId;
                updateIndex += _consumptionRepository.Edit(consump);
            }
            return updateIndex;
        }

        public List<VConsumption> GetConsumptionList(string orderStyleRefId)
        {
            return _consumptionRepository.GetConsumptions(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList();
        }

        public int DeleteConsumption(string consRefId, long consumptionId)
        {
            int deleteIndex = 0;
           bool usedStatus=   _compConsumptionRepository.Exists(x => x.ConsRefId == consRefId && x.CompId == _compId);
           if (usedStatus)
            {
                throw new Exception(message:"This item is used by Fabric consumption process");
            }
           else
           {
               using (var transaction = new TransactionScope())
               {
                   deleteIndex += _consumptionDetail.Delete(x => x.CompId == _compId && x.ConsRefId == consRefId);
                   deleteIndex += _consumptionRepository.Delete(x => x.CompId == _compId && x.ConsumptionId == consumptionId && x.ConsRefId == consRefId);
                   transaction.Complete();

               }
           }
          
            return deleteIndex;
        }

        public List<VwConsuptionOrderStyle> GetVwConsuptionOrderStyle(OM_BuyOrdStyle model, out int totalRecords)
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwConsuptionOrderStyle> vwConsuptionOrderStyles = _consumptionRepository.GetVwConsuptionStyle(_compId,employeeId,model.SearchString);
            vwConsuptionOrderStyles =vwConsuptionOrderStyles.Where(x => 
                                                   ((x.ShipDate >= model.FromDate || model.FromDate == null) &&
                                                    (x.ShipDate <= model.ToDate || model.ToDate == null)));
            totalRecords = vwConsuptionOrderStyles.Count();
            vwConsuptionOrderStyles = vwConsuptionOrderStyles
                      .OrderByDescending(r => r.OrderNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);

            return vwConsuptionOrderStyles.ToList();

        }

        public VConsumption GetFabricNameByConsRefId(string consRefId)
        {
          return  _consumptionRepository.GetConsumptions(x => x.CompId == _compId)
                .FirstOrDefault(x => x.ConsRefId == consRefId);
        }


        private int UpdateConsuptionDetails(OM_Consumption model)
        {
            var saveIndex = 0;
            var consumptionDetails = new List<OM_ConsumptionDetail>();
            switch (model.ConsTypeRefId)
            {
                case "1":
                    consumptionDetails = GeneralConsDetails(model.ConsRefId, model.OrderStyleRefId);
                    break;
                case "2":
                    consumptionDetails = ColorSizeConsDetails(model.ConsRefId, model.OrderStyleRefId);
                    break;
                case "3":
                    consumptionDetails = ColorConsDetails(model.ConsRefId, model.OrderStyleRefId);
                    break;
                case "4":
                    consumptionDetails = SizeConsDetails(model.ConsRefId, model.OrderStyleRefId);
                    break;
            }
            model.CompId = _compId;
            if (consumptionDetails.Any())
            {
                saveIndex += _consumptionDetail.SaveList(consumptionDetails);
            }
            return saveIndex;
        }
        private List<OM_ConsumptionDetail> ColorConsDetails(string consRefId, string orderStyleRefId)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var colorConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.ColorRefId).ToList().Select(y => new OM_ConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP), // Will QuantityP
                GColorRefId = y.First().ColorRefId,
                GSizeRefId = null,
                PColorRefId = y.First().ColorRefId,
                PSizeRefId = null,
                ConsRefId = consRefId,
                PAllow = 0.0M,
                TotalQty = 0,
                CompId = _compId
            }).ToList();
            return colorConsDetails;
        }
        private List<OM_ConsumptionDetail> SizeConsDetails(string consRefId, string orderStyleRefId)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var sizeConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.SizeRefId).ToList().Select(y => new OM_ConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = null,
                GSizeRefId = y.First().SizeRefId,
                PColorRefId = null,
                PSizeRefId = y.First().SizeRefId,
                ConsRefId = consRefId,
                PAllow = 0.0M,
                TotalQty = 0,
                CompId = _compId
            }).ToList();
            return sizeConsDetails;
        }

        private List<OM_ConsumptionDetail> ColorSizeConsDetails(string consRefId, string orderStyleRefId)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var colorSizeConsDetails = buyerBuyOrdShipDetails.GroupBy(x => new { x.SizeRefId, x.ColorRefId }).ToList().Select(y => new OM_ConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = y.First().ColorRefId,
                GSizeRefId = y.First().SizeRefId,
                PColorRefId = y.First().ColorRefId,
                PSizeRefId = y.First().SizeRefId,
                ConsRefId = consRefId,
                PAllow = 0.0M,
                TotalQty = 0,
                CompId = _compId
            }).ToList();
            return colorSizeConsDetails;
        }
        private List<OM_ConsumptionDetail> GeneralConsDetails(string consRefId, string orderStyleRefId)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var generalConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.OrderStyleRefId).ToList().Select(y => new OM_ConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = null,
                GSizeRefId = null,
                PColorRefId = null,
                PSizeRefId = null,
                ConsRefId = consRefId,
                PAllow = 0.0M,
                TotalQty = 0,
                CompId = _compId
            }).ToList();
            return generalConsDetails;
        }
    }
}
