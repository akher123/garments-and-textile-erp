using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class AccessoriesBookingController : BaseController
    {
        private readonly IPurchaseOrderManager _purchaseOrderManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IOmBuyerManager _buyerManager;
        public AccessoriesBookingController(IOmBuyerManager buyerManager,IPurchaseOrderManager purchaseOrderManager, IOmBuyOrdStyleManager omBuyOrdStyleManager)
        {
            _buyerManager = buyerManager;
            _purchaseOrderManager = purchaseOrderManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;

        }
        public ActionResult Index(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            List<VOMBuyOrdStyle> vomBuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyles(model, out totalRecords);
            model.OmBuyOrdStyles = vomBuyOrdStyles;
            model.TotalRecords = totalRecords;
            return View(model);
        }
 
        public ActionResult AccessoriesBookingList(PurchaseOrderViewModel model)
        {
           ModelState.Clear();
           const string accessoriesPTyle = "A";
            model.PurchaseOrder.PType = accessoriesPTyle;
            model.PurchaseOrders = _purchaseOrderManager.GetPurchaseOrderList(model.PurchaseOrder.PType, model.OrderStyleRefId, model.OrderNo);
            return PartialView("~/Areas/Commercial/Views/AccessoriesBooking/_AccessoriesBookingList.cshtml", model);
        }
      
        public ActionResult Edit(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            const string accessoriesPTyle = "A";
            model.PurchaseOrder.PurchaseOrderId = model.PurchaseOrderId;
            model.PurchaseOrder.OrderStyleRefId = model.OrderStyleRefId;
            model.PurchaseOrder.OrderNo = model.OrderNo;
            model.SupplierCompanies = _purchaseOrderManager.GetAssignedSuppliers(model.OrderStyleRefId);
            if (model.PurchaseOrder.PurchaseOrderId > 0)
            {
                model.PurchaseOrder = _purchaseOrderManager.GetPurchaseOrderById(model.PurchaseOrder.PurchaseOrderId);
                model.PurchaseOrderDetails = _purchaseOrderManager.GetAllPurchaseOrderDetails(model.PurchaseOrder.PurchaseOrderId).ToList();
                model.Details = model.PurchaseOrderDetails.ToDictionary(x => Convert.ToString(x.PurchaseOrderDetailId),
                 x => x);
            }
            else
            {
                model.PurchaseOrder.PurchaseOrderRefId = _purchaseOrderManager.GetNewPurchaseOrderRefId(accessoriesPTyle);

            
            }
            return View(model);

        }

        public ActionResult DetailBooking(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.PurchaseOrderDetails = _purchaseOrderManager.GetAllPurchaseOrderDetails(model.PurchaseOrderId).ToList();
            return View(model);
        }
        public ActionResult Save(PurchaseOrderViewModel model)
        {
            int saveIndex = 0;
            string errorMessage = "";
            const  string accessoriesPTyle = "A";
            try
            {

                model.PurchaseOrder.CommPurchaseOrderDetail = model.Details.Select(x=>x.Value).Select(x => new CommPurchaseOrderDetail()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    SizeRefId = x.SizeRefId ?? "0000",
                    ItemCode = x.ItemCode,
                    ColorRefId = x.ColorRefId??"0000",
                    GColorRefId = x.GColorRefId ?? "0000",
                    GSizeRefId = x.GSizeRefId ?? "0000",
                    xRate = x.xRate,
                    Quantity = x.Quantity,
                    PurchaseOrderRefId = model.PurchaseOrder.PurchaseOrderRefId
                }).ToList();

                model.PurchaseOrder.PType = accessoriesPTyle;
                saveIndex = model.PurchaseOrder.PurchaseOrderId > 0 ? _purchaseOrderManager.EditPurchaseOrder(model.PurchaseOrder) : _purchaseOrderManager.SavePurchaseOrder(model.PurchaseOrder);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                errorMessage = exception.Message;
            }
            if (saveIndex>0)
            {
                return RedirectToAction("AccessoriesBookingList", new { OrderStyleRefId = model.PurchaseOrder.OrderStyleRefId, OrderNo = model.PurchaseOrder.OrderNo });
            }
            else
            {
                return ErrorResult("System Error :" + errorMessage);
            }
        }
      
        public ActionResult Delete(PurchaseOrderViewModel model)
        {
            try
            {
                int deletedRows = _purchaseOrderManager.DeletePurchseOrder(model.PurchaseOrderId);
                if (deletedRows > 0)
                {
                    return RedirectToAction("AccessoriesBookingList", new { OrderStyleRefId = model.OrderStyleRefId, OrderNo = model.OrderNo });
                }
                else
                {
                    return ErrorResult("Delete Failed");
                }
            }
            catch (Exception exception)
            {

                return ErrorResult("Delete Failed" + exception.InnerException);
            }
          
          
        }
         
        public ActionResult GetBookedAccessoriesBySupplier(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.PurchaseOrderDetails = _purchaseOrderManager.GetAllAccessories(model.OrderStyleRefId, model.SupplierId).ToList();
            model.Details = model.PurchaseOrderDetails.ToDictionary(x => Convert.ToString(x.PurchaseOrderDetailId),
                x => x);
            return PartialView("~/Areas/Commercial/Views/AccessoriesBooking/_PurchaserOrderDetailList.cshtml", model);
        }

        public ActionResult AccessoriesBookingApproval(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            if (model.IsSearch)
            {
              
                const string accessoriesPTyle = "A";
                model.PurchaseOrder.PType = accessoriesPTyle;
                model.PurchaseOrders = _purchaseOrderManager.GetApprovalPurchaseOrderList(model.PurchaseOrder.PType, model.PurchaseOrder.IsApproved, model.SearchString, model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
                model.OrderList = _omBuyOrdStyleManager.GetOrderByBuyer(model.BuyerRefId);
                model.Styles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo); 
            }
            model.IsSearch = true;
            return View(model);
        }
    
        public JsonResult IsAccessoriesBookingApproval(long purchaseOrderId)
        {
            bool isApproved = _purchaseOrderManager.IsBookingApproed(purchaseOrderId);
            return Json(isApproved, JsonRequestBehavior.AllowGet);
        }

    }
}