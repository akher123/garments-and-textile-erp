using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class BuyerOrderManager : IBuyerOrderManager
    {
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly IOmBuyOrdStyleRepository _buyOrdStyleRepository;
        private readonly IBuyOrdShipRepository _ordShipRepository;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        private readonly IRepository<VOM_BuyOrdStyle> _buyerOrderStyle;
        private readonly string _compId;
        public BuyerOrderManager(IRepository<VOM_BuyOrdStyle> buyerOrderStyle,IUserMerchandiserRepository userMerchandiserRepository,IBuyerOrderRepository buyerOrderRepository,
            IOmBuyOrdStyleRepository buyOrdStyleRepository,IBuyOrdShipRepository ordShipRepository)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _buyOrdStyleRepository = buyOrdStyleRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _ordShipRepository = ordShipRepository;
            _compId = PortalContext.CurrentUser.CompId;
            this._buyerOrderStyle = buyerOrderStyle;
        }
        public List<VBuyerOrder> GetBuyerOrderPaging(string closed, int pageIndex, string sort, string sortdir, string searchString,DateTime? fromDate,DateTime?toDate, out int totalRecords)
        {
            var index = pageIndex;
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =  _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId&&x.CompId==_compId) .Select(x => x.MerchandiserRefId).ToArray();

               Expression<Func<VBuyerOrder, bool>> predicate = x => x.CompId == _compId&&(x.Closed==closed) &&permitedMerchandiserLsit.Contains(x.MerchandiserId)
                &&((x.OrderNo.Trim().Replace(" ", String.Empty).Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString.Trim().Replace(" ",String.Empty).ToLower()))
                ||(x.RefNo.Trim().Replace(" ", String.Empty).Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString.Trim().Replace(" ",String.Empty).ToLower()))
                  ||(x.BuyerName.Trim().Replace(" ", String.Empty).Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString.Trim().Replace(" ",String.Empty).ToLower()))
                 || (x.SeasonName.Trim().Replace(" ", String.Empty).Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString.Trim().Replace(" ",String.Empty).ToLower()))
                 || (x.EmpName.Trim().Replace(" ", String.Empty).Contains(searchString.Trim()) || String.IsNullOrEmpty(searchString.Trim().Replace(" ",String.Empty).ToLower())))
                 && ((x.OrderDate >= fromDate || fromDate == null) && (x.OrderDate <= toDate || toDate == null));

            var buyerOrders = _buyerOrderRepository.GetBuyerOrderViews(predicate);
            totalRecords = buyerOrders.Count();
            var pageSize = AppConfig.PageSize;
            switch (sort)
            {
                case "OrderNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                                  .OrderByDescending(r => r.OrderNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                  .OrderBy(r => r.OrderNo)
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                    }
                    break;
                case "RefNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                             .OrderByDescending(r => r.RefNo)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.RefNo)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                case "BuyerName":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                             .OrderByDescending(r => r.BuyerName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "BuyerRef":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                             .OrderByDescending(r => r.BuyerRef)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerRef)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "OrderDate":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                             .OrderByDescending(r => r.OrderDate)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.OrderDate)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;
                case "EmpName":
                    switch (sortdir)
                    {
                        case "DESC":
                            buyerOrders = buyerOrders
                             .OrderByDescending(r => r.EmpName)
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.EmpName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    buyerOrders = buyerOrders
                      .OrderByDescending(r => r.BuyerOrderId).ThenByDescending(x=>x.OrderDate)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;
            }
            return buyerOrders.ToList();
        }

        public List<VBuyerOrder> GetBuyerWithoutLcOrderPaging(VBuyerOrder model, out int totalRecords)
        {
            var OrderNoList = _buyerOrderStyle.Filter(x => x.StyleName.Contains(model.SearchString)).Select(x=>x.OrderNo);

            var index = model.PageIndex;
         
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =
                _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId)
                    .Select(x => x.MerchandiserRefId)
                    .ToArray();
            Expression<Func<VBuyerOrder, bool>> predicate = x => x.CompId == _compId && x.LcRefId == null && permitedMerchandiserLsit.Contains(x.MerchandiserId)
              
             && ((x.OrderNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
             || (x.RefNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
               || (x.BuyerName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
              || (x.SeasonName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
              || (x.EmpName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower())))
              ||(OrderNoList.Contains(x.OrderNo))
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
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                  .OrderBy(r => r.OrderNo)
                                  .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.RefNo)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerName)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerRef)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.OrderDate)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.EmpName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    buyerOrders = buyerOrders
                      .OrderByDescending(r => r.BuyerOrderId).ThenByDescending(x => x.OrderDate)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;
            }
            //   totalRecords = buyerOrders.Count();
            return buyerOrders.ToList();
        }
        public List<VBuyerOrder> GetBuyerLcOrderPaging(VBuyerOrder model, out int totalRecords)
        {
            var OrderNoList = _buyerOrderStyle.All().Where(x => x.StyleName.Contains(model.SearchString)).ToList();
            var index = model.PageIndex;
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =
                _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId)
                    .Select(x => x.MerchandiserRefId)
                    .ToArray();
            Expression<Func<VBuyerOrder, bool>> predicate = x => x.CompId == _compId && x.LcRefId !=null && permitedMerchandiserLsit.Contains(x.MerchandiserId)
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
                                  .Skip(index * pageSize)
                                  .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                  .OrderBy(r => r.OrderNo)
                                  .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.RefNo)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerName)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.BuyerRef)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.OrderDate)
                                 .Skip(index * pageSize)
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
                             .Skip(index * pageSize)
                             .Take(pageSize);
                            break;
                        default:
                            buyerOrders = buyerOrders
                                 .OrderBy(r => r.EmpName)
                                 .Skip(index * pageSize)
                                 .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    buyerOrders = buyerOrders
                      .OrderByDescending(r => r.BuyerOrderId).ThenByDescending(x => x.OrderDate)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;
            }
            //   totalRecords = buyerOrders.Count();
            return buyerOrders.ToList();
        }


        public int EditBuyerOrder(OM_BuyerOrder model)
        {
            var buyerOrder = _buyerOrderRepository.FindOne(x => x.BuyerOrderId == model.BuyerOrderId&&x.CompId==_compId);
            buyerOrder.RefNo = model.RefNo;
            buyerOrder.OrderNo = model.OrderNo;
            buyerOrder.OrderDate = model.OrderDate;
            buyerOrder.SampleOrdNo = model.SampleOrdNo;
            buyerOrder.BuyerRefId = model.BuyerRefId;
            buyerOrder.DGRefNo = model.DGRefNo;
            buyerOrder.BuyerRef = model.BuyerRef;
            buyerOrder.Quantity = model.Quantity;
            buyerOrder.OAmount = model.OAmount;
            buyerOrder.MerchandiserId = model.MerchandiserId;
            buyerOrder.AgentRefId = model.AgentRefId;
            buyerOrder.ShipagentRefId = model.ShipagentRefId;
            buyerOrder.ConsigneeRefId = model.ConsigneeRefId;
            buyerOrder.PayTermRefId = model.PayTermRefId;
            buyerOrder.OrderTypeRefId = model.OrderTypeRefId;
            buyerOrder.SMode = model.SMode;
            buyerOrder.Fab = model.Fab;
            buyerOrder.SCont = model.SCont;
            buyerOrder.SeasonRefId = model.SeasonRefId;
            buyerOrder.OrderStatus = model.OrderStatus;
            buyerOrder.Remarks = model.Remarks;
            return _buyerOrderRepository.Edit(buyerOrder);
        }

        public int SaveBuyerOrder(OM_BuyerOrder model)
        {
            model.Closed = "O";
            model.OrderNo = _buyerOrderRepository.GetNewRefNo(_compId);
            model.BasUnit = 26;
            model.CompId = _compId;
            model.RefDate = DateTime.Now;
           return _buyerOrderRepository.Save(model);
        }

        public int EditLcOrder(OM_BuyerOrder model)
        {          
            return _buyerOrderRepository.Edit(model);
        }
        public OM_BuyerOrder GetBuyerOrderById(long buyerOrderId)
        {
            return _buyerOrderRepository.FindOne(x => x.BuyerOrderId == buyerOrderId && x.CompId == _compId);
        }

        public string GetNewRefNo()
        {
            return _buyerOrderRepository.GetNewRefNo(_compId);
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

        public VBuyerOrder GetBuyerOrderByOrderNo(string orderNo)
        {
            return _buyerOrderRepository.GetBuyerOrderViews(x=>x.OrderNo==orderNo&&x.CompId==_compId).FirstOrDefault();
        }

        public OM_BuyerOrder GetBuyerLcOrderByOrderNo(string orderNo)
        {
            return _buyerOrderRepository.FindOne(x => x.OrderNo.Trim().ToLower() == orderNo.Trim().ToLower() && x.CompId == _compId);
        }

        public IEnumerable GetOrderByBuyer(string buyerRefId)
        {
            return _buyerOrderRepository.Filter(x => x.BuyerRefId == buyerRefId && x.CompId == _compId);
        }

        public int UpdateOrderStatus(long buyerOrderId, string closed)
        {
            var buyerOrder=  _buyerOrderRepository.FindOne(x => x.BuyerOrderId == buyerOrderId);
            buyerOrder.Closed = closed;
            buyerOrder.CloseDate = DateTime.Now;
            int satatus = buyerOrder.Closed == "O" ?1:0;
            int updated=  _buyOrdStyleRepository.CloseAllStyleByOrder(buyerOrder.OrderNo, satatus, buyerOrder.CompId);
            return _buyerOrderRepository.Edit(buyerOrder);
        }


        public OrderSheet GetOrderSheet(string orderNo)
        {
            var orderSheet=new OrderSheet
            {
                VBuyerOrder =
                    _buyerOrderRepository.GetBuyerOrderViews(x => x.CompId == _compId)
                        .FirstOrDefault(x => x.OrderNo == orderNo)??new VBuyerOrder()
            };
            if (orderSheet.VBuyerOrder!=null)
            {
                var vomBuyOrdStyles = _buyOrdStyleRepository.GetBuyerOrderStyle(x => x.CompId == _compId && x.OrderNo == orderSheet.VBuyerOrder.OrderNo).ToList() ?? new List<VOMBuyOrdStyle>();
                foreach (var ordStyle in vomBuyOrdStyles)
                {
                    var orderStyle = new OrderStyle
                    {
                        BuyOrdStyle = ordStyle,
                        OrderShipTable = UpdateTempAssort(ordStyle.OrderStyleRefId)
                    };
                    orderSheet.OrderStyles.Add(ordStyle.OrderStyleRefId, orderStyle);
                }
            }
           
            return orderSheet;
        }

        public IEnumerable<VBuyerOrder> GetVBuyerOrder(VBuyerOrder model)
        {
            Expression<Func<VBuyerOrder, bool>> predicate = x => x.CompId == _compId&& ((x.OrderDate >= model.FromDate || model.FromDate == null) && (x.OrderDate <= model.ToDate || model.ToDate == null));
            var buyerOrders = _buyerOrderRepository.GetBuyerOrderViews(predicate);
            return buyerOrders.ToList();
        }

        public DataTable GetMerchaiserWiseOrderDataTable(DateTime? fromDate, DateTime? toDate)
        {
            return _buyerOrderRepository.GetMerchaiserWiseOrderDataTable(fromDate, toDate);
        }

        public IEnumerable<VOM_BuyOrdStyle> GetOrderDetailsByOrderNo(string orderNo)
        {
            return _buyerOrderStyle.Filter(x=>x.OrderNo.Contains(orderNo)).ToList();
        }

        private DataTable UpdateTempAssort(string orderStyleRefId)
        {
            try
            {
                DataTable tAssortTable;
                using (var transaction = new TransactionScope())
                {
                    var orderShip = new OM_BuyOrdShip()
                    {
                        OrderStyleRefId = orderStyleRefId,
                        CompId = _compId
                    };

                    tAssortTable = _ordShipRepository.UpdateTempAssort(orderShip);

                    var ordStyleSizeView = _ordShipRepository.GetBuyOrdStyleSize(orderShip);
                    foreach (var size in ordStyleSizeView)
                    {
                        tAssortTable.Columns["C" + size.SizeRow].ColumnName = size.SizeName.Replace(" ", String.Empty);
                    }
                    var tablelength = ordStyleSizeView.Count;
                    var numOfCols = tAssortTable.Columns.Count - 6;
                    for (var i = tablelength + 1; i < numOfCols; i++)
                    {
                        tAssortTable.Columns.Remove("C" + i);
                    }
                    tAssortTable.Columns.Remove("TempAssortId");
                    tAssortTable.Columns.Remove("CompId");
                    tAssortTable.Columns.Remove("ColorRow");
                    tAssortTable.Columns.Remove("UserId");
                    tAssortTable.Columns.Remove("ColorRefId");
                    var toInsert = tAssortTable.NewRow();
                    toInsert[0] = "Total:";
                    for (var i = 1; i < tAssortTable.Columns.Count; i++)
                    {
                        toInsert[i] =
                            tAssortTable.Compute("sum([" + tAssortTable.Columns[i].ColumnName + "])", "").ToString();
                    }
                    tAssortTable.Rows.InsertAt(toInsert, tAssortTable.Rows.Count + 1);
                    transaction.Complete();
                }
                return tAssortTable;


            }
            catch (Exception exception)
            {

                throw new Exception("Style Wise size and Color with Qty not entry properly!");
            }
        }


    }
}
