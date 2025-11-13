using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OrderTypeManager : IOrderTypeManager
    {
        private readonly IOrderTypeRepository _orderTypeRepository;
        private readonly string _compId;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        public OrderTypeManager(IOrderTypeRepository orderTypeRepository, IBuyerOrderRepository buyerOrderRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _orderTypeRepository = orderTypeRepository;
            _buyerOrderRepository = buyerOrderRepository;
          
        }

        public List<OM_OrderType> GetOrdertypes()
        {
            return _orderTypeRepository.All().OrderBy(x=>x.OTypeName).ToList();
        }

        public List<OM_OrderType> GetOrderTypesByPaging(OM_OrderType model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var oTypes= _orderTypeRepository.Filter(x => 
              ((x.OTypeName.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))
                || (x.OTypeRefId.Trim().Replace(" ", string.Empty).ToLower().Contains(model.SearchString.Trim().Replace(" ", string.Empty).ToLower()) || String.IsNullOrEmpty(model.SearchString))));
            totalRecords = oTypes.Count();
            switch (model.sort)
            {
                case "OTypeName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            oTypes = oTypes
                                .OrderByDescending(r => r.OTypeName).ThenBy(x=>x.Prefix)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            oTypes = oTypes
                                .OrderBy(r => r.OTypeName).ThenBy(x => x.Prefix)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "OTypeRefId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            oTypes = oTypes
                                .OrderByDescending(r => r.OTypeRefId).ThenBy(x => x.Prefix)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            oTypes = oTypes
                                .OrderBy(r => r.OTypeRefId).ThenBy(x => x.Prefix)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    oTypes = oTypes
                        .OrderByDescending(r => r.OTypeRefId).ThenBy(x => x.Prefix)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return oTypes.ToList();
        }

        public OM_OrderType GetOrderTypeById(int orderTypeId)
        {
            return _orderTypeRepository.FindOne(x => x.OrderTypeId == orderTypeId);
        }

        public string GetNewOTypeRefId()
        {

            return _orderTypeRepository.GetNewOTypeRefId();
        }

        public int EditOrderType(OM_OrderType model)
        {
            var season = _orderTypeRepository.FindOne(x => x.OrderTypeId == model.OrderTypeId);
            season.OTypeName = model.OTypeName;
            season.Prefix = model.Prefix;
            return _orderTypeRepository.Edit(season);
        }

        public int SaveOrderType(OM_OrderType model)
        {

            model.OTypeRefId = _orderTypeRepository.GetNewOTypeRefId();
            return _orderTypeRepository.Save(model);
        }

        public int DeleteOType(string oTypeRefId)
        {
            var isUsesd = _buyerOrderRepository.Exists(x => x.OrderTypeRefId == oTypeRefId && x.CompId == _compId);
            var deleted = 0;
            if (isUsesd)
            {
                deleted = -1;
            }
            else
            {
                deleted = _orderTypeRepository.Delete(x => x.OTypeRefId == oTypeRefId);
            }
            return deleted;
        }

        public bool CheckExistingOrderType(OM_OrderType model)
        {
           return _orderTypeRepository.Exists(
                x => x.OrderTypeId != model.OrderTypeId && x.OTypeName.ToLower().Equals(model.OTypeName.ToLower()));
        }
    }
}
