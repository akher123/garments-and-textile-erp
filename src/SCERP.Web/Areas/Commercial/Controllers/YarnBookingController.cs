using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class YarnBookingController : BaseController
    {      private readonly IPurchaseOrderManager _purchaseOrderManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        private readonly IProFormaInvoiceManager proFormaInvoiceManager;
        private IOmBuyerManager _omBuyerManager;
        public YarnBookingController(IProFormaInvoiceManager proFormaInvoiceManager, IPurchaseOrderManager purchaseOrderManager, IOmBuyOrdStyleManager omBuyOrdStyleManager, IOmBuyerManager omBuyerManager)
        {
            _purchaseOrderManager = purchaseOrderManager;
            _omBuyOrdStyleManager = omBuyOrdStyleManager;
            _omBuyerManager = omBuyerManager;
            this.proFormaInvoiceManager = proFormaInvoiceManager;

        }
        public ActionResult Index(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
        //    model.FabricOrderDetails = _fabricOrderManager.GetVwFabricOrders(model.PageIndex, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult YarnBookingList(PurchaseOrderViewModel model)
        {
           ModelState.Clear();
            model.PurchaseOrder.PType = "Y";
            model.PurchaseOrders = _purchaseOrderManager.GetPurchaseOrderList(model.PurchaseOrder.PType, model.OrderStyleRefId, model.OrderNo);
            return PartialView("~/Areas/Commercial/Views/YarnBooking/_YarnBookingList.cshtml", model);
        }
        public ActionResult Edit(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.PurchaseOrder.PurchaseOrderId = model.PurchaseOrderId;
            model.PurchaseOrder.OrderStyleRefId = model.OrderStyleRefId;
            model.PurchaseOrder.OrderNo = _omBuyOrdStyleManager.GetBuyOrdStyleByRefId(model.OrderStyleRefId).OrderNo;
            model.SupplierCompanies = _purchaseOrderManager.GetQuitedYarnSupplier(model.OrderStyleRefId);
            model.PurchaseOrder.PType = "Y";
            if (model.PurchaseOrder.PurchaseOrderId > 0)
            {
                model.PurchaseOrder = _purchaseOrderManager.GetPurchaseOrderById(model.PurchaseOrder.PurchaseOrderId);
                model.ProFormaInvoices= proFormaInvoiceManager.GetPiBySupplier(model.PurchaseOrder.SupplierId, PortalContext.CurrentUser.CompId);
                model.PurchaseOrderDetails = _purchaseOrderManager.GetAllPurchaseOrderDetails(model.PurchaseOrder.PurchaseOrderId).ToList();
            }
            else
            {
                model.PurchaseOrder.PurchaseOrderRefId = _purchaseOrderManager.GetNewPurchaseOrderRefId(model.PurchaseOrder.PType);

            }
            return View(model);

        }

        public ActionResult Save(PurchaseOrderViewModel model)
        {
            int saveIndex = 0;
            string errorMessage = "";
            try
            {

                model.PurchaseOrder.CommPurchaseOrderDetail = model.PurchaseOrderDetails.Select(x => new CommPurchaseOrderDetail()
                {
                    CompId = PortalContext.CurrentUser.CompId,
                    SizeRefId = x.SizeRefId,
                    GColorRefId = x.GColorRefId,
                    ItemCode = x.ItemCode,
                    ColorRefId = x.ColorRefId,
                    xRate = x.xRate,
                    Quantity = x.Quantity,
                    PurchaseOrderRefId = model.PurchaseOrder.PurchaseOrderRefId
                }).ToList();
                model.PurchaseOrder.PType = "Y";

                saveIndex = model.PurchaseOrder.PurchaseOrderId > 0 ? _purchaseOrderManager.EditPurchaseOrder(model.PurchaseOrder) : _purchaseOrderManager.SavePurchaseOrder(model.PurchaseOrder);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                errorMessage = exception.Message;
            }
            if (saveIndex>0)
            {
                return RedirectToAction("YarnBookingList", new { OrderStyleRefId = model.PurchaseOrder.OrderStyleRefId, OrderNo = model.PurchaseOrder.OrderNo });
            }
            else
            {
                return ErrorResult("System Error :" + errorMessage);
            }
        


        }
        public ActionResult Delete(PurchaseOrderViewModel model)
        {
            int deletedRows = _purchaseOrderManager.DeletePurchseOrder(model.PurchaseOrderId);
            if (deletedRows > 0)
            {
                return RedirectToAction("YarnBookingList", new { OrderStyleRefId = model.OrderStyleRefId, OrderNo = model.OrderNo });
            }
            else
            {
                return ErrorResult("System Error");
            }
            return Reload();
        }

        public ActionResult GetBookedYarnsBySupplier(PurchaseOrderViewModel model)
        {
            model.PurchaseOrderDetails = _purchaseOrderManager.GetAllRateQuittedYarns(model.OrderStyleRefId, model.SupplierId, model.PiRefId).ToList();
            return PartialView("~/Areas/Commercial/Views/YarnBooking/_PurchaserOrderDetailList.cshtml", model);
        }
        public ActionResult YarnBookingApproval(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _omBuyerManager.GetAllBuyers();
            const string accessoriesPTyle = "Y";
            model.PurchaseOrder.PType = accessoriesPTyle;
            model.PurchaseOrders = _purchaseOrderManager.GetApprovalPurchaseOrderList(model.PurchaseOrder.PType, model.PurchaseOrder.IsApproved, model.SearchString,model.BuyerRefId,model.OrderNo,model.OrderStyleRefId);
            return View(model);
        }

        public ActionResult ApprovedYarnBooking(PurchaseOrderViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _omBuyerManager.GetAllBuyers();
            const string accessoriesPTyle = "Y";
            model.PurchaseOrder.IsApproved = true;
            model.PurchaseOrder.PType = accessoriesPTyle;
            model.PurchaseOrders = _purchaseOrderManager.GetApprovalPurchaseOrderList(model.PurchaseOrder.PType, model.PurchaseOrder.IsApproved, model.SearchString, model.BuyerRefId, model.OrderNo, model.OrderStyleRefId);
            return View(model);
        }
	}
}