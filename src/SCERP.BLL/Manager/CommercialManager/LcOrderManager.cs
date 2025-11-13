using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;


namespace SCERP.BLL.Manager.CommercialManager
{
    public class LcOrderManager:ILcOrderManager
    {
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private readonly IBuyOrdShipRepository _ordShipRepository;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        private readonly string _compId;

        public LcOrderManager(IUserMerchandiserRepository userMerchandiserRepository, IBuyerOrderRepository buyerOrderRepository, IOmBuyOrdStyleRepository buyOrdStyleRepository, IBuyOrdShipRepository ordShipRepository)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _ordShipRepository = ordShipRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<VBuyerOrder> GetBuyerOrderPaging(VBuyerOrder model, out int totalRecords)
        {
            var index = model.PageIndex;
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =
                _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId)
                    .Select(x => x.MerchandiserRefId)
                    .ToArray();
            Expression<Func<VBuyerOrder, bool>> predicate = x => x.CompId == _compId && permitedMerchandiserLsit.Contains(x.MerchandiserId)
                                                                 && ((x.OrderNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
                                                                     || (x.RefNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
                                                                     || (x.BuyerName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
                                                                     || (x.SeasonName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
                                                                     || (x.EmpName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower())))
                                                                 && ((x.OrderDate >= model.FromDate || model.FromDate == null) && (x.OrderDate <= model.ToDate || model.ToDate == null));

            var buyerOrders = _buyerOrderRepository.GetBuyerOrderViews(predicate);
            totalRecords = buyerOrders.Count();
            var pageSize = AppConfig.PageSize;

            switch (model.sort)
            {
                case "OrderNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.OrderNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.OrderNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "RefNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.RefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.RefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BuyerName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.BuyerName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.BuyerName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BuyerRef":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.BuyerRef)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.BuyerRef)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "OrderDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.OrderDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.OrderDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "EmpName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                .OrderByDescending(r => r.EmpName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                .OrderBy(r => r.EmpName)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    buyerOrders = buyerOrders
                        .OrderByDescending(r => r.BuyerOrderId).ThenByDescending(x => x.OrderDate)
                        .Skip(index*pageSize)
                        .Take(pageSize);
                    break;
            }
            return buyerOrders.ToList();
        }

        public int SaveBuyerOrder(OM_BuyerOrder model)
        {
            model.CloseDate = DateTime.Now;
            model.OrderNo = _buyerOrderRepository.GetNewRefNo(_compId);
            model.BasUnit = 26;
            model.CompId = _compId;
            model.RefDate = DateTime.Now;
            return _buyerOrderRepository.Save(model);
        }

        public OM_BuyerOrder GetBuyerOrderById(long buyerOrderId)
        {
            return _buyerOrderRepository.FindOne(x => x.BuyerOrderId == buyerOrderId && x.CompId == _compId);
        }

        public int DeleteBuyerOrder(string orderNo)
        {
            var deleteStatus = 0;
            var isProcessed = _buyOrdStyleRepository.Exists(x => x.OrderNo == orderNo && x.CompId == _compId);
            if (isProcessed)
            {
                deleteStatus = -1;
            }
            else
            {
                deleteStatus = _buyerOrderRepository.Delete(x => x.OrderNo == orderNo && x.CompId == _compId);
            }
            return deleteStatus;
        }
    }
}
