using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class OmBuyOrdStyleManager : IOmBuyOrdStyleManager
    {
        private readonly IOmBuyOrdStyleRepository _omBuyOrdStyleRepository;
        private readonly IBuyOrdStyleColorRepository _buyOrdStyleColorRepository;
        private readonly IBuyOrdStyleSizeRepository _buyOrdStyleSizeRepository;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        private readonly ITimeAndActionManager _timeAndActionManager;
        private readonly IBuyerTnaTemplateRepository _buyerTnaTemplateRepository;
        private readonly string _compId;
        private readonly IUserMerchandiserRepository _userMerchandiserRepository;
        private readonly ISmsManager _smsManager;
        private readonly IEmailTemplateUserManager _emailTemplateUser;
        public OmBuyOrdStyleManager(IUserMerchandiserRepository userMerchandiserRepository, IOmBuyOrdStyleRepository omBuyOrdStyleRepository, IBuyerOrderRepository buyerOrderRepository, IBuyOrdStyleColorRepository buyOrdStyleColorRepository, IBuyOrdStyleSizeRepository buyOrdStyleSizeRepository, ITimeAndActionManager timeAndActionManager, IBuyerTnaTemplateRepository buyerTnaTemplateRepository, ISmsManager smsManager, IEmailTemplateUserManager emailTemplateUser)
        {
            _userMerchandiserRepository = userMerchandiserRepository;
            _omBuyOrdStyleRepository = omBuyOrdStyleRepository;
            _buyerOrderRepository = buyerOrderRepository;
            _buyOrdStyleColorRepository = buyOrdStyleColorRepository;
            _compId = PortalContext.CurrentUser.CompId;

            _buyOrdStyleSizeRepository = buyOrdStyleSizeRepository;
            _timeAndActionManager = timeAndActionManager;
            _buyerTnaTemplateRepository = buyerTnaTemplateRepository;
            _smsManager = smsManager;
            _emailTemplateUser = emailTemplateUser;
        }
        public List<VOMBuyOrdStyle> GetBuyerOrderStyleByOrderNo(string orderNo)
        {
            var buyerOrders = _omBuyOrdStyleRepository.GetBuyerOrderStyle(x => x.OrderNo == orderNo && x.CompId == _compId);
            return buyerOrders.ToList();
        }

        public OM_BuyOrdStyle GetBuyOrdStyleById(long orderStyleId)
        {
            return _omBuyOrdStyleRepository.FindOne(x => x.OrderStyleId == orderStyleId && x.CompId == _compId);
        }

        public string GetNewStyleRefNo()
        {
            return _omBuyOrdStyleRepository.GetStyleRefNo(_compId);
        }

        public int EditBuyOrdStyle(OM_BuyOrdStyle model)
        {
            var buyerOrderStyle = _omBuyOrdStyleRepository.FindOne(x => x.OrderStyleId == model.OrderStyleId && x.CompId == _compId);
            buyerOrderStyle.OrderStyleId = model.OrderStyleId;
            buyerOrderStyle.OrderStyleRefId = model.OrderStyleRefId;
            buyerOrderStyle.OrderNo = model.OrderNo;
            buyerOrderStyle.SeasonRefId = model.SeasonRefId;
            buyerOrderStyle.StyleentDate = model.StyleentDate;
            buyerOrderStyle.StyleRefId = model.StyleRefId;
            buyerOrderStyle.BuyerArt = model.BuyerArt;
            buyerOrderStyle.BrandRefId = model.BrandRefId;
            buyerOrderStyle.Quantity = model.Quantity;
            buyerOrderStyle.CatIRefId = model.CatIRefId;
            buyerOrderStyle.Rate = model.Rate;
            buyerOrderStyle.Discount = model.Discount;
            buyerOrderStyle.Amt = model.Amt;
            buyerOrderStyle.Rmks = model.Rmks;
            buyerOrderStyle.ImagePath = model.ImagePath;
            buyerOrderStyle.PI = model.PI;
            buyerOrderStyle.LCSTID = model.LCSTID;
            int index = 0;
             index = _omBuyOrdStyleRepository.Edit(buyerOrderStyle);
            if (!_timeAndActionManager.Exist(buyerOrderStyle.OrderStyleRefId, buyerOrderStyle.CompId))
            {
                index = CreateTna(buyerOrderStyle);
            }

            //var buyerOrder = _buyerOrderRepository.FindOne(x => x.CompId == _compId && x.OrderNo == model.OrderNo);
            //bool existTemplate = _buyerTnaTemplateRepository.Exists(x => x.CompId == _compId && x.BuyerRefId == buyerOrder.BuyerRefId);
            //if (existTemplate)
            //{
            //    _timeAndActionManager.Delete(buyerOrderStyle.OrderStyleRefId, _compId);
            //    index = _timeAndActionManager.CreateTnaByBuyerTemplate(_compId, buyerOrder.BuyerRefId, (int)TemplateType.Template1, model.OrderStyleRefId, buyerOrder.OrderDate.GetValueOrDefault());
            //    var orderStyle = _omBuyOrdStyleRepository.GetBuyerOrderStyle(_compId, buyerOrderStyle.OrderStyleRefId);
            //    List<string> phones = _emailTemplateUser.GetEmailTemplateUsersPhoneNumbers(EmailTemplateRefId.MANAGEMENT_TNA, PortalContext.CurrentUser.CompId);
            //    _smsManager.Send(phones, String.Format("TNA has been created for Buyer:{0} of Order:{1} and Style:{2}", orderStyle.BuyerName, orderStyle.RefNo, orderStyle.StyleName));
            //}
            //else
            //{
            //    index = _timeAndActionManager.CreateTnaByActivityTemplate(model.OrderStyleRefId);
            //}

            return index;
        }

        public int SaveBuyOrdStyle(OM_BuyOrdStyle model)
        {
            model.CompId = _compId;
            model.ActiveStatus = true;
            model.TnaMode = "U";
            model.OrderStyleRefId = _omBuyOrdStyleRepository.GetStyleRefNo(_compId);
            int index = _omBuyOrdStyleRepository.Save(model);
            if (index > 0)
            {
                index = CreateTna(model);
            }
            return index;

        }

        private int CreateTna(OM_BuyOrdStyle model)
        {
            int index = 0;
            var buyerOrder = _buyerOrderRepository.FindOne(x => x.CompId == _compId && x.OrderNo == model.OrderNo);
            if (!buyerOrder.SCont.Equals("S"))
            {

                bool existTemplate = _buyerTnaTemplateRepository.Exists(x => x.CompId == _compId && x.BuyerRefId == buyerOrder.BuyerRefId);

                if (existTemplate)
                {
                    index = _timeAndActionManager.CreateTnaByBuyerTemplate(_compId, buyerOrder.BuyerRefId,
                        (int)TemplateType.Template1, model.OrderStyleRefId, buyerOrder.OrderDate.GetValueOrDefault());
                }
                else
                {
                    index = _timeAndActionManager.CreateTnaByActivityTemplate(model.OrderStyleRefId);

                }
                if (index > 0)
                {
                    var orderStyle = _omBuyOrdStyleRepository.GetBuyerOrderStyle(_compId, model.OrderStyleRefId);
                    List<string> phones = _emailTemplateUser.GetEmailTemplateUsersPhoneNumbers(EmailTemplateRefId.MANAGEMENT_TNA, PortalContext.CurrentUser.CompId);
                    _smsManager.Send(phones, String.Format("TNA has been created for Buyer:{0} of Style:{1}", orderStyle.BuyerName, orderStyle.StyleName));
                }

            }
            else
            {
                index = 1;
            }
            return index;
        }

        public int DeleteBuyerOrderStyle(string orderStyleRefId)
        {

            var isDelete = 0;
            var isProcessToColor = _buyOrdStyleColorRepository.Exists(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            var isProcessToSize = _buyOrdStyleSizeRepository.Exists(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
            if (isProcessToSize || isProcessToColor)
            {
                isDelete = -1;
            }
            else
            {
                isDelete = _omBuyOrdStyleRepository.Delete(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                _timeAndActionManager.Delete(orderStyleRefId, _compId);
            }
            return isDelete;
        }

        public object StyleAutocomplite(string searchString)
        {
            var styleList = _omBuyOrdStyleRepository.GetVBuyerOrderStyle(_compId);
            return styleList.Where(
                x =>
                    ((x.OrderNo.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                     (x.BuyerName.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                        (x.Merchandiser.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||

                        (x.BuyerRef.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                          (x.OrderStyleRefId.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                          (x.StyleRefId.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                        (x.StyleName.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||
                     (x.BuyerArt.Trim()
                        .Replace(" ", string.Empty)
                        .ToLower()
                        .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString))
                     ||

                     (x.RefNo.Trim()
                         .Replace(" ", string.Empty)
                         .ToLower()
                         .Contains(searchString.Trim().Replace(" ", string.Empty).ToLower()) ||
                      String.IsNullOrEmpty(searchString)))).Take(8);

        }

        public OM_BuyOrdStyle GetBuyOrdStyleByRefId(string orderStyleRefId)
        {
            return _omBuyOrdStyleRepository.FindOne(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
        }

        public VOMBuyOrdStyle GetVBuyOrdStyleByRefId(string orderStyleRefId)
        {
            var buyerOrders = _omBuyOrdStyleRepository.GetBuyerOrderStyle(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId).FirstOrDefault();
            return buyerOrders;
        }

        public List<VOMBuyOrdStyle> GetBuyerOrderStyles(OM_BuyOrdStyle model, out int totalRecords)
        {
            var employeeId = PortalContext.CurrentUser.UserId;
            var permitedMerchandiserLsit =
                _userMerchandiserRepository.Filter(x => x.IsActive && x.EmployeeId == employeeId && x.CompId == _compId)
                    .Select(x => x.MerchandiserRefId)
                    .ToArray();
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VOMBuyOrdStyle, bool>> predicate = x => x.CompId == _compId && permitedMerchandiserLsit.Contains(x.MerchandiserId)
                && ((x.OrderNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
               || (x.RefNo.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
               || (x.StyleName.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower()))
               || (x.OrderStyleRefId.Trim().Replace(" ", String.Empty).Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString.Trim().Replace(" ", String.Empty).ToLower())))
               && ((x.StyleentDate >= model.FromDate || model.FromDate == null) && (x.StyleentDate <= model.ToDate || model.ToDate == null));
            var buyerOrders = _omBuyOrdStyleRepository.GetBuyerOrderStyle(predicate);
            totalRecords = buyerOrders.Count();
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

                default:
                    buyerOrders = buyerOrders
                      .OrderByDescending(r => r.OrderNo)
                      .Skip(index * pageSize)
                      .Take(pageSize);
                    break;

            }

            return buyerOrders.ToList();


        }

        public bool CheckGreaterQty(string orderNo, decimal qty)
        {
            decimal orderQty = _buyerOrderRepository.FindOne(x => x.OrderNo == orderNo && x.CompId == _compId).Quantity ?? 0;
            decimal totalStyleQty = _omBuyOrdStyleRepository.Filter(x => x.OrderNo == orderNo && x.CompId == _compId).ToList().Sum(x => x.Quantity.GetValueOrDefault());
            return (orderQty >= totalStyleQty + qty);
        }

        public List<VwStyleFollowupStatus> GetStyleFollowupStatusesByPaging(int pageIndex, DateTime? fromDate, DateTime? toDate, string merchandiserId,
            string buyerRefId, string searchString, out int totalRecords)
        {
            IQueryable<VwStyleFollowupStatus> styleFollowupStatuses = _omBuyOrdStyleRepository.GetStyleFollowupStatusesByPaging(buyerRefId, searchString);
            totalRecords = styleFollowupStatuses.Count();
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            styleFollowupStatuses = styleFollowupStatuses
                     .OrderByDescending(r => r.ShipDate)
                     .Skip(index * pageSize)
                     .Take(pageSize);
            return styleFollowupStatuses.ToList();
        }

        public IEnumerable GetOrderByBuyer(string buyerRefId)
        {
            return _buyerOrderRepository.GetBuyerOrderViews(
                   x => x.CompId == PortalContext.CurrentUser.CompId && x.Closed != "C" && x.BuyerRefId == buyerRefId).Select(x => new
                   {
                       OrderNo = x.OrderNo,
                       RefNo = x.RefNo,
                   }).ToList();
        }

        public IEnumerable GetStyleByOrderNo(string orderNo)
        {
            return _omBuyOrdStyleRepository.GetBuyerOrderStyle(
            x => x.CompId == PortalContext.CurrentUser.CompId && x.OrderNo == orderNo&&x.ActiveStatus==true).Select(x => new
            {
                OrderStyleRefId = x.OrderStyleRefId,
                StyleNo = x.StyleName
            }).ToList();
        }

        public IEnumerable GetColorsByOrderStyleRefId(string orderStyleRefId)
        {
            return _buyOrdStyleColorRepository.GetBuyOrdStyleColor(orderStyleRefId, PortalContext.CurrentUser.CompId);
        }

        public int SaveShipQty(OM_BuyOrdStyle model)
        {
            var orderstyle = _omBuyOrdStyleRepository.FindOne(x =>
                      x.OrderNo == model.OrderNo && x.OrderStyleId == model.OrderStyleId &&
                      x.CompId == PortalContext.CurrentUser.CompId);
            orderstyle.ActiveStatus = model.ActiveStatus;
            orderstyle.despatchQty = model.despatchQty;
            return _omBuyOrdStyleRepository.Edit(orderstyle);
        }

        public IEnumerable GetOrderAllByBuyer(string buyerRefId)
        {
            return _buyerOrderRepository.GetBuyerOrderViews(
                 x => x.CompId == PortalContext.CurrentUser.CompId && x.BuyerRefId == buyerRefId).Select(x => new
                 {
                     OrderNo = x.OrderNo,
                     RefNo = x.RefNo,
                 }).ToList();
        }

        public List<VOM_BuyOrdStyle> GetBuyerOrderStyles(int pageIndex, string buyerRefId, string orderNo, string orderStyleRefId, string isLocked, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var buyerOrders = _omBuyOrdStyleRepository.GetVBuyerOrderStyle(compId: _compId);
            buyerOrders =
                buyerOrders.Where(x => x.ActiveStatus && x.TnaMode == isLocked && x.CompId == _compId && (x.BuyerRefId == buyerRefId || string.IsNullOrEmpty(buyerRefId)) &&
                        (x.OrderNo == orderNo || String.IsNullOrEmpty(orderNo)) &&
                        (x.OrderStyleRefId == orderStyleRefId || String.IsNullOrEmpty(orderStyleRefId)));
            totalRecords = buyerOrders.Count();
            buyerOrders = buyerOrders
                  .OrderByDescending(r => r.BuyerName)
                  .Skip(index * pageSize)
                  .Take(pageSize);
            return buyerOrders.ToList();
        }

        public int TnaApproved(int orderStyleId, string compId)
        {
            OM_BuyOrdStyle ordStyle = _omBuyOrdStyleRepository.FindOne(x => x.OrderStyleId == orderStyleId);
            ordStyle.TnaMode = ordStyle.TnaMode == "L" ? "U" : "L";
            return _omBuyOrdStyleRepository.Edit(ordStyle);
        }

        public IEnumerable GetSizeByOrderStyleRefId(string orderStyleRefId)
        {
            return _buyOrdStyleColorRepository.GetSizeByOrderStyleRefId(orderStyleRefId, PortalContext.CurrentUser.CompId);
        }
    }
}
