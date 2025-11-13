using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;


using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class CompConsumptionManager : ICompConsumptionManager
    {
        private readonly string _compId;
        private readonly ICompConsumptionRepository _compConsumptionRepository;
        private readonly IConsumptionDetailRepository _consumptionDetailRepository;
        private readonly ICompConsumptionDetailRepository _compConsumptionDetailRepository;
        private readonly IConsumptionRepository _consumption;
        private readonly IBuyOrdShipDetailRepository _buyOrdShipDetailRepository;
        public CompConsumptionManager(IBuyOrdShipDetailRepository buyOrdShipDetailRepository, IConsumptionRepository consumption, IConsumptionDetailRepository consumptionDetailRepository, ICompConsumptionRepository compConsumptionRepository, ICompConsumptionDetailRepository compConsumptionDetailRepository)
        {
            _buyOrdShipDetailRepository = buyOrdShipDetailRepository;
            _consumptionDetailRepository = consumptionDetailRepository;
            _compConsumptionDetailRepository = compConsumptionDetailRepository;
            _consumption = consumption;
            _compId = PortalContext.CurrentUser.CompId;
            _compConsumptionRepository = compConsumptionRepository;

        }

        public List<VCompConsumption> GetCompConsumption(string orderStyleRefId)
        {
            IQueryable<VCompConsumption> compConsumptions = _compConsumptionRepository.GetVCompConsumptions(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).OrderBy(x => x.ComponentSlNo);
            return compConsumptions.ToList();
        }

        public int GetNewCompConsuotionSlNo(string orderStyleRefId)
        {

            var compList = _compConsumptionRepository.Filter(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            return compList.Any() ? compList.Max(x => x.ComponentSlNo + 1) : 1;
        }
#region important code

        //public int EditCompConsumption(OM_CompConsumption model)
        //{
        //    var saveIndex = 0;
        //    using (var transaction = new TransactionScope())
        //    {
        //        var compConsumption =
        //            _compConsumptionRepository.FindOne(
        //                x => x.CompConsumptionId == model.CompConsumptionId && x.CompId == _compId);
        //        _compConsumptionDetailRepository.Delete(
        //         x =>
        //             x.CompID == _compId && x.ConsRefId == compConsumption.ConsRefId &&
        //             x.CompomentSlNo == compConsumption.ComponentSlNo &&
        //             x.OrderStyleRefId == compConsumption.OrderStyleRefId);

        //        var fabConsDetail = _consumptionDetailRepository.GetVConsumptionDetails(compConsumption.ConsRefId, compConsumption.CompId);
        //        var colorConsDetails = fabConsDetail.Select(x => new OM_CompConsumptionDetail()
        //        {
        //            QuantityP = x.QuantityP,
        //            GColorRefId = x.GColorRefId,
        //            GSizeRefId = x.GSizeRefId,
        //            PColorRefId = x.PColorRefId,
        //            PSizeRefId = x.PSizeRefId,
        //            ConsRefId = x.ConsRefId,
        //            CompID = x.CompId,
        //            OrderStyleRefId = model.OrderStyleRefId,
        //            CompomentSlNo = model.ComponentSlNo,
        //            PAllow = 0.0M,
        //            PPQty = 0.0M,
        //            GSM = 0.0M,
        //            Length = 0.0M,
        //            Width = 0.0M,
        //            TQty = 0.0M,
        //            BaseColorRefId = "0000",
        //        }).ToList();

        //        compConsumption.CompConsumptionId = model.CompConsumptionId;
        //        compConsumption.ComponentSlNo = model.ComponentSlNo;
        //        compConsumption.ConsRefId = model.ConsRefId;
        //        compConsumption.ComponentRefId = model.ComponentRefId;
        //        compConsumption.NParts = model.NParts;
        //        compConsumption.FabricType = model.FabricType;
        //        compConsumption.EnDate = model.EnDate;
        //        compConsumption.OrderStyleRefId = model.OrderStyleRefId;
        //        compConsumption.Description = model.Description;
        //        saveIndex += _compConsumptionRepository.Edit(compConsumption);
        //        saveIndex += _compConsumptionDetailRepository.SaveList(colorConsDetails);
        //        transaction.Complete();

        //    }
        //    return saveIndex;
        //}
        //public int SaveCompConsumption(OM_CompConsumption model)
        //{
        //    var saveIndex = 0;
        //    bool isExist =
        //        _compConsumptionRepository.Exists(x => x.CompId == _compId && x.ConsRefId == model.ConsRefId && x.OrderStyleRefId == model.OrderStyleRefId && x.ComponentRefId == model.ComponentRefId);
        //    if (!isExist)
        //    {
        //        using (var transaction = new TransactionScope())
        //        {
        //            model.CompId = _compId;
        //            saveIndex = _compConsumptionRepository.Save(model);

        //            var fabConsDetail = _consumptionDetailRepository.GetVConsumptionDetails(model.ConsRefId,
        //            model.CompId);

        //            var colorConsDetails = fabConsDetail.Select(x => new OM_CompConsumptionDetail()
        //            {
        //                QuantityP = x.QuantityP,
        //                GColorRefId = x.GColorRefId,
        //                GSizeRefId = x.GSizeRefId,
        //                PColorRefId = x.PColorRefId,
        //                PSizeRefId = x.PSizeRefId,
        //                ConsRefId = x.ConsRefId,
        //                CompID = x.CompId,
        //                OrderStyleRefId = model.OrderStyleRefId,
        //                CompomentSlNo = model.ComponentSlNo,
        //                PAllow = 0.0M,
        //                PPQty = 0.0M,
        //                GSM = 0.0M,
        //                Length = 0.0M,
        //                Width = 0.0M,
        //                TQty = 0.0M,
        //                BaseColorRefId = null,
        //            }).ToList();

        //            saveIndex += _compConsumptionDetailRepository.SaveList(colorConsDetails);
        //            transaction.Complete();

        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(message: "Componet alredy added");
        //    }

        //    return saveIndex;
        //}
        #endregion
        public int SaveCompConsumption(OM_CompConsumption model)
        {
            var saveIndex = 0;
            bool isExist =
                _compConsumptionRepository.Exists(x => x.CompId == _compId && x.ConsRefId == model.ConsRefId && x.OrderStyleRefId == model.OrderStyleRefId && x.ComponentRefId == model.ComponentRefId);
            if (!isExist)
            {
                using (var transaction = new TransactionScope())
                {
                    model.CompId = _compId;
                    OM_Consumption consumption =
                        _consumption.FindOne(x => x.CompId == _compId && x.ConsRefId == model.ConsRefId);
                    saveIndex = _compConsumptionRepository.Save(model);

                    UpdateComConsuptionDetails(consumption,model.ComponentSlNo);
                    transaction.Complete();

                }
            }
            else
            {
                throw new Exception(message: "Componet alredy added");
            }

            return saveIndex;
        }

        private int UpdateComConsuptionDetails(OM_Consumption model,int componentSlNo)
        {
            var saveIndex = 0;
            var consumptionDetails = new List<OM_CompConsumptionDetail>();
            switch (model.ConsTypeRefId)
            {
                case "1":
                    consumptionDetails = GeneralComConsDetails(model.ConsRefId, model.OrderStyleRefId, componentSlNo);
                    break;
                case "2":
                    consumptionDetails = ColorSizeComConsDetails(model.ConsRefId, model.OrderStyleRefId, componentSlNo);
                    break;
                case "3":
                    consumptionDetails = ColorComConsDetails(model.ConsRefId, model.OrderStyleRefId, componentSlNo);
                    break;
                case "4":
                    consumptionDetails = SizeComConsDetails(model.ConsRefId, model.OrderStyleRefId, componentSlNo);
                    break;
            }
            model.CompId = _compId;
            if (consumptionDetails.Any())
            {
                saveIndex += _compConsumptionDetailRepository.SaveList(consumptionDetails);
            }
            return saveIndex;
        }
        private List<OM_CompConsumptionDetail> SizeComConsDetails(string consRefId, string orderStyleRefId,int componentSlNo)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var sizeConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.SizeRefId).ToList().Select(y => new OM_CompConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = null,
                GSizeRefId = y.First().SizeRefId,
                PColorRefId = null,
                PSizeRefId = y.First().SizeRefId,
                ConsRefId = consRefId,
              
                CompID = _compId,
                OrderStyleRefId = orderStyleRefId,
                CompomentSlNo = componentSlNo,
                PAllow = 0.0M,
                PPQty = 0.0M,
                GSM = 0.0M,
                Length = 0.0M,
                Width = 0.0M,
                TQty = 0.0M,
                BaseColorRefId = "0000",
            }).ToList();
            return sizeConsDetails;
        }

        private List<OM_CompConsumptionDetail> GeneralComConsDetails(string consRefId, string orderStyleRefId, int componentSlNo)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var generalConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.OrderStyleRefId).ToList().Select(y => new OM_CompConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = "0000",
                GSizeRefId = "0000",
                PColorRefId = "0000",
                PSizeRefId = "0000",
                ConsRefId = consRefId,
                CompID = _compId,
                OrderStyleRefId = orderStyleRefId,
                CompomentSlNo = componentSlNo,
                PAllow = 0.0M,
                PPQty = 0.0M,
                GSM = 0.0M,
                Length = 0.0M,
                Width = 0.0M,
                TQty = 0.0M,
                BaseColorRefId = null,
            }).ToList();
            return generalConsDetails;
        }

        private List<OM_CompConsumptionDetail> ColorSizeComConsDetails(string consRefId, string orderStyleRefId, int componentSlNo)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var colorSizeConsDetails = buyerBuyOrdShipDetails.GroupBy(x => new { x.SizeRefId, x.ColorRefId }).ToList().Select(y => new OM_CompConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP),// Will QuantityP
                GColorRefId = y.First().ColorRefId,
                GSizeRefId = y.First().SizeRefId,
                PColorRefId = y.First().ColorRefId,
                PSizeRefId = y.First().SizeRefId,
                ConsRefId = consRefId,
                CompID = _compId,
                OrderStyleRefId = orderStyleRefId,
                CompomentSlNo = componentSlNo,
                PAllow = 0.0M,
                PPQty = 0.0M,
                GSM = 0.0M,
                Length = 0.0M,
                Width = 0.0M,
                TQty = 0.0M,
                BaseColorRefId = null,
            }).ToList();
            return colorSizeConsDetails;
        }


        private List<OM_CompConsumptionDetail> ColorComConsDetails(string consRefId, string orderStyleRefId, int componentSlNo)
        {
            var buyerBuyOrdShipDetails = _buyOrdShipDetailRepository.GetVBuyOrdShipDetail(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var colorConsDetails = buyerBuyOrdShipDetails.GroupBy(x => x.ColorRefId).ToList().Select(y => new OM_CompConsumptionDetail()
            {
                QuantityP = y.Sum(x => x.QuantityP), // Will QuantityP
                GColorRefId = y.First().ColorRefId,
                GSizeRefId = "0000",
                PColorRefId = y.First().ColorRefId,
                PSizeRefId = "0000",
                ConsRefId = consRefId,
                CompID = _compId,
                OrderStyleRefId = orderStyleRefId,
                CompomentSlNo = componentSlNo,
                PAllow = 0.0M,
                PPQty = 0.0M,
                GSM = 0.0M,
                Length = 0.0M,
                Width = 0.0M,
                TQty = 0.0M,
                BaseColorRefId = "0000",
            }).ToList();
            return colorConsDetails;
        }
        public OM_CompConsumption GetCompConsumptionById(long compConsumptionId)
        {
            return _compConsumptionRepository.FindOne(x => x.CompConsumptionId == compConsumptionId && x.CompId == _compId);
        }


        public int EditCompConsumption(OM_CompConsumption model)
        {
            var saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var compConsumption =
                    _compConsumptionRepository.FindOne(
                        x => x.CompConsumptionId == model.CompConsumptionId && x.CompId == _compId);
                _compConsumptionDetailRepository.Delete(
                 x =>
                     x.CompID == _compId && x.ConsRefId == compConsumption.ConsRefId &&
                     x.CompomentSlNo == compConsumption.ComponentSlNo &&
                     x.OrderStyleRefId == compConsumption.OrderStyleRefId);

                OM_Consumption consumption =
                        _consumption.FindOne(x => x.CompId == _compId && x.ConsRefId == model.ConsRefId);
                saveIndex += UpdateComConsuptionDetails(consumption, model.ComponentSlNo);
                compConsumption.CompConsumptionId = model.CompConsumptionId;
                compConsumption.ComponentSlNo = model.ComponentSlNo;
                compConsumption.ConsRefId = model.ConsRefId;
                compConsumption.ComponentRefId = model.ComponentRefId;
                compConsumption.NParts = model.NParts;
                compConsumption.FabricType = model.FabricType;
                compConsumption.EnDate = model.EnDate;
                compConsumption.OrderStyleRefId = model.OrderStyleRefId;
                compConsumption.Description = model.Description;
                saveIndex += _compConsumptionRepository.Edit(compConsumption);
             
                transaction.Complete();

            }
            return saveIndex;
        }


        public int DeleteCompConsumption(long compConsumptionId)
        {
            int delteIndex = 0;
            OM_CompConsumption consumption = _compConsumptionRepository.FindOne(x => x.CompConsumptionId == compConsumptionId && x.CompId == _compId);
            if (consumption == null)
            {
                throw new NullReferenceException("Consumption Not deleted");
            }
            using (var transaction = new TransactionScope())
            {
                delteIndex += _compConsumptionRepository.DeleteOne(consumption);
                delteIndex += _compConsumptionDetailRepository.Delete(x => x.OrderStyleRefId == consumption.OrderStyleRefId && x.CompomentSlNo == consumption.ComponentSlNo && x.CompID == _compId);
                transaction.Complete();
            }

            return delteIndex;

        }

        public List<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(OM_BuyOrdStyle model, out int totalRecords)
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwCompConsumptionOrderStyle> vCompConsumptionOrderStyle = _compConsumptionRepository.GetVwCompConsumptionOrderStyle(_compId, employeeId, model.SearchString);
            vCompConsumptionOrderStyle =
                vCompConsumptionOrderStyle.Where(x =>
                ((x.ShipDate >= model.FromDate || model.FromDate == null) &&
                 (x.ShipDate <= model.ToDate || model.ToDate == null)));
            totalRecords = vCompConsumptionOrderStyle.Count();
            vCompConsumptionOrderStyle = vCompConsumptionOrderStyle
                      .OrderByDescending(r => r.OrderNo).ThenBy(x => x.OrderStyleRefId)
                      .Skip(index * pageSize)
                      .Take(pageSize);
            return vCompConsumptionOrderStyle.ToList();
        }

        public VCompConsumption GetVCompConsumptionById(long compConsumptionId)
        {
            return _compConsumptionRepository.GetVCompConsumptions(x => x.CompId == _compId).FirstOrDefault(x => x.CompConsumptionId == compConsumptionId);
        }
    }


}

