using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class FabricOrderManager : IFabricOrderManager
    {
        private readonly IFabricOrderRepository _fabricOrderRepository;
        private readonly string _compId;
        private readonly FabricOrderDetailRepository _fabricOrderDetail;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;

        public FabricOrderManager(IUserMerchandiserRepository userMerchandiserRepository, IFabricOrderRepository fabricOrderRepository, FabricOrderDetailRepository fabricOrderDetail)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _fabricOrderRepository = fabricOrderRepository;
            _fabricOrderDetail = fabricOrderDetail;
            _compId = PortalContext.CurrentUser.CompId;
        }
        public List<VwFabricOrder> GetFabricOrderByPaging(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate,
            string status, string searchString, out int totalRecords)
        {
            status = status ?? "P";
            var employeeId = PortalContext.CurrentUser.UserId;
            var merc = _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId).Select(x => x.MerchandiserRefId).ToArray();
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var fabricOrderList = _fabricOrderRepository.GetVwFabricOrders(x => x.CompId == _compId && merc.Contains(x.MerchandiserRefId)
               && (x.StyleName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) && (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) && ((x.Status == status)));
            totalRecords = fabricOrderList.Count();
            switch (sort)
            {
                case "FabricOrderRefId":
                    switch (sortdir)
                    {
                        case "DESC":
                            fabricOrderList = fabricOrderList
                                .OrderByDescending(r => r.FabricOrderRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            fabricOrderList = fabricOrderList
                                .OrderBy(r => r.FabricOrderRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ExpDate":
                    switch (sortdir)
                    {
                        case "DESC":
                            fabricOrderList = fabricOrderList
                                .OrderByDescending(r => r.ExpDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            fabricOrderList = fabricOrderList
                                .OrderBy(r => r.ExpDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    fabricOrderList = fabricOrderList
                        .OrderByDescending(r => r.ExpDate)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return fabricOrderList.ToList();
        }

        public OM_FabricOrder GetFabricOrderbyId(int fabricOrderId)
        {
            return _fabricOrderRepository.FindOne(x => x.FabricOrderId == fabricOrderId && x.CompId == _compId);
        }

        public string GetFabricOrderRefId()
        {

            var refIdDesgit = _fabricOrderRepository.Filter(x => x.CompId == _compId).Max(x => x.FabricOrderRefId.Substring(2)) ?? "0";
            string ordRefId = "FO" + refIdDesgit.IncrementOne().PadZero(5);
            return ordRefId;
        }

        public int EditFabricOrder(OM_FabricOrder fabricOrder)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                OM_FabricOrder fabricOrd = _fabricOrderRepository.FindOne(x => x.FabricOrderId == fabricOrder.FabricOrderId && x.CompId == _compId);
                if (fabricOrd == null)
                {
                    throw new ArgumentNullException(@"fabricOrder Not found");
                }
                fabricOrd.SupplierId = fabricOrder.SupplierId;
                fabricOrd.OrderDate = fabricOrder.OrderDate;
                fabricOrd.BuyerRefId = fabricOrder.BuyerRefId;
                fabricOrd.ExpDate = fabricOrder.ExpDate;
                fabricOrd.Remarks = fabricOrder.Remarks;
                _fabricOrderDetail.Delete(x => x.CompId == _compId && x.FabricOrderId == fabricOrd.FabricOrderId);
                foreach (var orderDetail in fabricOrder.OM_FabricOrderDetail)
                {
                    orderDetail.FabricOrderId = fabricOrder.FabricOrderId;
                    orderDetail.YLocked = false;
                    _fabricOrderDetail.Save(orderDetail);
                }
                edited = _fabricOrderRepository.Edit(fabricOrd);
                transaction.Complete();
            }
            return edited;
        }

        public int SaveFabricOrder(OM_FabricOrder fabricOrder)
        {
            fabricOrder.CompId = _compId;
            fabricOrder.Status = "P";
            fabricOrder.PreparedBy = PortalContext.CurrentUser.UserId;
            var merc = _userMerchandiserRepository.FindOne(x => x.EmployeeId == fabricOrder.PreparedBy && x.CompId == _compId);
            fabricOrder.MerchandiserRefId = merc.MerchandiserRefId;
            fabricOrder.OM_FabricOrderDetail = fabricOrder.OM_FabricOrderDetail.Select(x =>
            {
                x.YLocked = false;
                return x;
            }).ToList();
            return _fabricOrderRepository.Save(fabricOrder);
        }

        public int DeleteFabricOrder(int fabricOrderId)
        {
            _fabricOrderDetail.Delete(x => x.FabricOrderId == fabricOrderId);
            return _fabricOrderRepository.Delete(x => x.FabricOrderId == fabricOrderId);
        }

        public List<VwCompConsumptionOrderStyle> GeFabricConsStyleList(string orderNo)
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            return _fabricOrderRepository.GeFabricConsStyleList(orderNo, employeeId, _compId);
        }

        public List<VwCompConsumptionOrderStyle> GeFabricOrderDetail(int fabricOrderId)
        {
            return _fabricOrderRepository.GeFabricOrderDetailById(fabricOrderId, _compId);
        }

        public int UpdateFabricOrderStatus(string status, int fabricOrderId)
        {
            var fabricOrder = _fabricOrderRepository.FindOne(x => x.FabricOrderId == fabricOrderId);
            fabricOrder.Status = status;
            return _fabricOrderRepository.Edit(fabricOrder);

        }
        public List<VwFabricOrderDetail> GetVwFabricOrders(int pageIndex, string buyerRefId, string orderNo, string orderStyleRefId, out int totalRecords)
        {
            IQueryable<VwFabricOrderDetail> fabricOrderDetails = _fabricOrderDetail.GetVwFabricOrders(x => x.CompId == PortalContext.CurrentUser.CompId && x.ActiveStatus == "A"
             && (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo))
             && (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId))
             && (x.BuyerRefId == buyerRefId || String.IsNullOrEmpty(buyerRefId)));
            var pageSize = AppConfig.PageSize;
            totalRecords = fabricOrderDetails.Count();
            fabricOrderDetails = fabricOrderDetails
                            .OrderByDescending(x => x.FabricOrderRefId)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize);
            return fabricOrderDetails.ToList();
        }
        public List<VwFabricOrderDetail> GetVwFabricOrders(int pageIndex, string searchString, out int totalRecords)
        {
            IQueryable<VwFabricOrderDetail> fabricOrderDetails =
                _fabricOrderDetail.GetVwFabricOrders(x => x.CompId == PortalContext.CurrentUser.CompId && x.ActiveStatus == "A"
                 && ((x.StyleName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.StyleName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
                 || (x.OrderName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.OrderName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
                 || (x.OrderStyleRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.OrderStyleRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
                 || ((x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)))));
            var pageSize = AppConfig.PageSize;
            totalRecords = fabricOrderDetails.Count();
            fabricOrderDetails = fabricOrderDetails
                            .OrderByDescending(x => x.FabricOrderRefId)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize);
            return fabricOrderDetails.ToList();
        }

        public List<VwFabricOrder> GetApprovedFabricOrders(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate,
            string status, string searchString, out int totalRecords)
        {

            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var fabricOrderList = _fabricOrderRepository.GetVwFabricOrders(x => x.CompId == _compId && x.Status == status
               && ((x.StyleName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
               || (x.OrderName.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
               || (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString)) || (x.FabricOrderRefId.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))));
            totalRecords = fabricOrderList.Count();
            switch (sort)
            {
                case "FabricOrderRefId":
                    switch (sortdir)
                    {
                        case "DESC":
                            fabricOrderList = fabricOrderList
                                .OrderByDescending(r => r.FabricOrderRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            fabricOrderList = fabricOrderList
                                .OrderBy(r => r.FabricOrderRefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "ExpDate":
                    switch (sortdir)
                    {
                        case "DESC":
                            fabricOrderList = fabricOrderList
                                .OrderByDescending(r => r.ExpDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            fabricOrderList = fabricOrderList
                                .OrderBy(r => r.ExpDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    fabricOrderList = fabricOrderList
                        .OrderByDescending(r => r.ExpDate)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return fabricOrderList.ToList();
        }

        public int FabricBookingLock(string orderStyleRefId, string compId)
        {

            var issue = _fabricOrderDetail.FindOne(x => x.CompId == compId && x.OrderStyleRefId == orderStyleRefId);
            if (issue.YLocked == false)
            {
                issue.LockedBy = PortalContext.CurrentUser.UserId;
                issue.LockedDate = DateTime.Now;
                issue.YLocked = true;
            }
            else
            {
                issue.UnLockedBy = PortalContext.CurrentUser.UserId;
                issue.UnLockedDate = DateTime.Now;
                issue.YLocked = false;
            }

            return _fabricOrderDetail.Edit(issue);
        }

        public bool IsFabricBookingLock(string orderStyleRefId, string compId)
        {
          return  _fabricOrderDetail.Exists(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == compId);
        }

        public bool IsFabricBookingLock(int fabricOrderId)
        {
          return  _fabricOrderDetail.Exists(x => x.FabricOrderId == fabricOrderId && x.YLocked);
        }
    }
}
