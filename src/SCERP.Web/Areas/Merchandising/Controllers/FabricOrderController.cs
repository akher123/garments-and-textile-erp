using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class FabricOrderController : BaseController
    {
        private readonly IFabricOrderManager _fabricOrderManager;
        private readonly ISupplierCompanyManager _supplier;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IBuyerOrderManager _orderManager;

        public FabricOrderController(IFabricOrderManager fabricOrderManager,ISupplierCompanyManager supplier,IOmBuyerManager buyerManager,IBuyerOrderManager orderManager)
        {
            _fabricOrderManager = fabricOrderManager;
            _supplier = supplier;
            _buyerManager = buyerManager;
            _orderManager=orderManager;
        }

        public ActionResult Index(FabricOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.FabricOrders = _fabricOrderManager.GetFabricOrderByPaging(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.FabricOrder.Status,model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(FabricOrderViewModel model)
        {
            ModelState.Clear();
            if (model.FabricOrder.FabricOrderId>0)
            {
               if(!_fabricOrderManager.IsFabricBookingLock(model.FabricOrder.FabricOrderId)){
                    model.FabricOrder = _fabricOrderManager.GetFabricOrderbyId(model.FabricOrder.FabricOrderId);
                    model.OmBuyOrdStyles = _fabricOrderManager.GeFabricOrderDetail(model.FabricOrder.FabricOrderId);
                }
                else
                {
                    return ErrorResult("This Fabric Booking is loked by purchase department!");
                }
       
            }
            else
            {
                model.FabricOrder.OrderDate = DateTime.Now;
                model.FabricOrder.FabricOrderRefId = _fabricOrderManager.GetFabricOrderRefId();
            }
            model.SuppliersList = _supplier.GetAllSupplierCompany();
            model.Buyers = _buyerManager.GetAllBuyers();
            model.OrderList = _orderManager.GetOrderByBuyer(model.FabricOrder.BuyerRefId);
            return View(model);
        }

        public ActionResult Save(FabricOrderViewModel model)
        {
            int saved = 0;
            if (model.FabricOrder.OM_FabricOrderDetail.Any())
            {
                saved = model.FabricOrder.FabricOrderId > 0 ? _fabricOrderManager.EditFabricOrder(model.FabricOrder) : _fabricOrderManager.SaveFabricOrder(model.FabricOrder); 
            }
            else
            {
                return ErrorResult("Select at least one style for order");
            }
           
            return saved>0 ? Reload() : ErrorMessageResult();
        }

        public ActionResult StyleListByOrder([Bind(Include = "FabricOrder")]FabricOrderViewModel model)
        {
            model.OmBuyOrdStyles = _fabricOrderManager.GeFabricConsStyleList(model.FabricOrder.OrderNo);
            return PartialView("~/Areas/Merchandising/Views/FabricOrder/_StyleList.cshtml", model);
        }
        public ActionResult Delete(int fabricOrderId)
        {
            int deleted = 0;
            if (!_fabricOrderManager.IsFabricBookingLock(fabricOrderId))
            {
                 deleted = _fabricOrderManager.DeleteFabricOrder(fabricOrderId);
            }
            else
            {
                return ErrorResult("This Fabric Booking is loked by purchase department!");
            }
  
            return deleted > 0 ? Reload() : ErrorResult("Delete Failed !");
           
        }

        public JsonResult UpdateFabricOrderStatus(string status, int fabricOrderId)
        {
            int index = _fabricOrderManager.UpdateFabricOrderStatus(status, fabricOrderId);
            if (index>0)
            {
                return ErrorResult("Status Successfully updated");
            }
            else
            {
                return ErrorResult("Failed to update Status");
            }
        }

        public ActionResult ApprovedFabricOrder(FabricOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.FabricOrder.Status = "A";
            model.FabricOrders = _fabricOrderManager.GetApprovedFabricOrders(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.FabricOrder.Status,model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        
       public ActionResult FabricBookingLock(string orderStyleRefId)
        {
            int locked = _fabricOrderManager.FabricBookingLock(orderStyleRefId,PortalContext.CurrentUser.CompId );
            return ErrorResult(locked > 0 ? "Fabric Booking Locked/UnLoked Successfully !" : "Lock/UnLock Failed !");
        }

    }
}