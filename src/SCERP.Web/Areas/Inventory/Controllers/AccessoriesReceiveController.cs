using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class AccessoriesReceiveController : BaseController
    {
        private readonly IMaterialReceiveAgainstPoManager _materialReceiveAgainstPoManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly IBookingManager _bookingManager;
        private readonly IPurchaseOrderManager _purchaseOrder;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        public AccessoriesReceiveController(IPurchaseOrderManager purchaseOrder, IOmBuyerManager omBuyerManager,
            IMaterialReceiveAgainstPoManager materialReceiveAgainstPoManager,
            ISupplierCompanyManager supplierCompanyManager, IBookingManager bookingManager, IOmBuyOrdStyleManager buyOrdStyle)
        {
            _purchaseOrder = purchaseOrder;
            _materialReceiveAgainstPoManager = materialReceiveAgainstPoManager;
            _supplierCompanyManager = supplierCompanyManager;
            _omBuyerManager = omBuyerManager;
            _bookingManager = bookingManager;
            _buyOrdStyle = buyOrdStyle;
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-1,accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult Index(ReceiveAgainstPoViewModel model)
        {

            ModelState.Clear();
            string[] types = new[] { RType.PI };
            const int acessoryStore = (int)StoreType.Acessories;
            int totalRecords;
            model.ReceiveAgainstPos = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByPaging(model.PageIndex,
                model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, out totalRecords, acessoryStore, types);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult Edit(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId > 0)
            {
                var receiveAgainstPo =
                    _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(
                        model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
                model.ReceiveAgainstPo = receiveAgainstPo;
                OM_Buyer buyer = _omBuyerManager.GetBuyerById(model.ReceiveAgainstPo.BuyerId.GetValueOrDefault());
                model.PiBookings = _purchaseOrder.GetPurchaseOrderByType("A", model.ReceiveAgainstPo.OrderStyleRefId);
                model.BuyerRefId = buyer.BuyerRefId;
                model.OrderList = _buyOrdStyle.GetOrderByBuyer(buyer.BuyerRefId);
                model.StyleList = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.ReceiveAgainstPo.OrderNo);
                model.Dictionary =_materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            }
            else
            {
                model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewAcessRcvRefId();
                model.ReceiveAgainstPo.MRRDate = DateTime.Now;
                model.ReceiveAgainstPo.InvoiceDate = DateTime.Now;
                model.ReceiveAgainstPo.GateEntryDate = DateTime.Now;
                model.ReceiveAgainstPo.ReceiveRegDate = DateTime.Now;
                model.ReceiveAgainstPo.PoDate = DateTime.Now;
            }

            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            return View(model);
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult Save(ReceiveAgainstPoViewModel model)
        {

            var savedIndex = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.CompId = compId;
                if (!model.Dictionary.Any())
                {
                    return ErrorResult("Please Add at least one item");
                }
                else
                {
                    model.ReceiveAgainstPo.StoreId = (int)StoreType.Acessories;
                    model.ReceiveAgainstPo.MRRDate = model.ReceiveAgainstPo.ReceiveRegDate;
                    model.ReceiveAgainstPo.MRRNo = model.ReceiveAgainstPo.ReceiveRegNo;
                    OM_Buyer buyer =
                        _omBuyerManager.GetBuyerByRefId(model.BuyerRefId, PortalContext.CurrentUser.CompId);
                    model.ReceiveAgainstPo.BuyerId = buyer.BuyerId;
                    var booking = _purchaseOrder.GetPurchaseOrderByRefId(model.ReceiveAgainstPo.PoNo);
                    model.ReceiveAgainstPo.PoDate = booking.BookingDate;
                    model.ReceiveAgainstPo.SupplierId = booking.SupplierId;
                    model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                        model.Dictionary.Select(x => x.Value).Where(x=>x.ReceivedQty>0).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                        {
                            CompId = compId,
                            ItemId = x.ItemId,
                            ColorRefId = x.ColorRefId,
                            SizeRefId = x.SizeRefId,
                            FColorRefId = x.FColorRefId,
                            GSizeRefId = x.GSizeRefId,
                            ReceivedQty = x.ReceivedQty,
                            ReceivedRate = x.ReceivedRate,
                            PurchaseOrderDetailId = x.PurchaseOrderDetailId,
                            Location = x.Location

                        }).ToList();
                }
                if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId == 0)
                {
                    model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewAcessRcvRefId();
                }
                savedIndex = _materialReceiveAgainstPoManager.SaveReceiveAgainstPo(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
           
            if (savedIndex>0)
            {
                var refNo = _materialReceiveAgainstPoManager.GetNewAcessRcvRefId();

                return Json(new { IsFailed = true, RefNo = refNo, Message = "Successfully Saved" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new { IsFailed = false, RefNo = "",Message="Saived Failed" }, JsonRequestBehavior.AllowGet);
                
            }
        }

        public ActionResult AddNewRow(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.Dictionary.Add(model.Key, model.PoDetail);
            return View("~/Areas/Inventory/Views/AccessoriesReceive/AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-3")]
        public ActionResult Delete(long materialReceiveAgstPoId)
        {
            int delteIndex = 0;
            try
            {
                var accessoriesReceve = (int)ActionType.AccessoriesReceive;
                delteIndex = _materialReceiveAgainstPoManager.DeteteReceiveAgainstPo(materialReceiveAgstPoId, accessoriesReceve);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return delteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult EditQc(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.QcDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult EditGrn(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.GrnDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult UpdateGrn(ReceiveAgainstPoViewModel model)
        {
            int updated = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                    model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                    {
                        CompId = compId,
                        ItemId = x.ItemId,
                        ColorRefId = x.ColorRefId,
                        SizeRefId = x.SizeRefId,
                        ReceivedQty = x.ReceivedQty,
                        ReceivedRate = x.ReceivedRate,
                        RejectedQty = x.RejectedQty,
                        MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                        MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId

                    }).ToList();
                model.ReceiveAgainstPo.GrnStatus = true;
                const int accessoriesReceve = (int)ActionType.AccessoriesReceive;
                updated = _materialReceiveAgainstPoManager.UpdateGrn(model.ReceiveAgainstPo, accessoriesReceve);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }

        [AjaxAuthorize(Roles = "accessoriesreceive-2,accessoriesreceive-3")]
        public ActionResult UpdateQc(ReceiveAgainstPoViewModel model)
        {
            int updated = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                    model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                    {
                        CompId = compId,
                        RejectedQty = x.RejectedQty,
                        MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                        MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId
                    }).ToList();
                model.ReceiveAgainstPo.QcStatus = true;
                updated = _materialReceiveAgainstPoManager.UpdateQc(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }
        public ActionResult PiBookingList(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();

         
            switch (model.RType)
            {
                case "P":
                
                    model.Dictionary = _purchaseOrder.GetAllPurchaseOrderDetailsByRefId(model.PiBookingRefId);
                    break;
                case "B":
                    
                    model.Dictionary = _bookingManager.GetVwYarnBookingDetail(model.PiBookingRefId);
                    break;
            }
            return PartialView("~/Areas/Inventory/Views/AccessoriesReceive/AddNewRow.cshtml", model);

        }
        public ActionResult PiBookingListByStyle(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            model.Dictionary = _purchaseOrder.GetAllPurchaseOrderDetailsByStyleRefId(model.OrderStyleRefId);
            return PartialView("~/Areas/Inventory/Views/AccessoriesReceive/AddNewRow.cshtml", model);

        }

        public ActionResult PiBooking(ReceiveAgainstPoViewModel model)
        {
            if (model.RType == "B")
            {
                const int storeId = (int)StoreType.Acessories;

                model.PiBookings = _bookingManager.GetBooking(storeId);

            }
            else
            {
                model.PiBookings = _purchaseOrder.GetPurchaseOrderByType("A",model.OrderStyleRefId);
            }
            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            model.ReceiveAgainstPo.PoDate = DateTime.Now;
            return View("~/Areas/Inventory/Views/AccessoriesReceive/_RType.cshtml", model);
        }
        public ActionResult MaterialReceivedAgainstPoOrBookingReport(long materialReceiveAgstPoId, string key)
        {

            List<VwMaterialReceiveAgainstPoDetail> receiveAgainstPoDetails = _materialReceiveAgainstPoManager.GetVwMaterialReceiveAgainstPoDetail(materialReceiveAgstPoId);
            string reportName = "";
            switch (key)
            {
                case "GRN":
                    reportName = "AccGrnReport";
                    break;
                case "QC":
                    reportName = "AccQcReport";
                    break;
                default:
                    reportName = "AccReceivedPoOrBookingReport";
                    break;
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), reportName + ".rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RcvAgPoBKDSet", receiveAgainstPoDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult GetAccessoriesRcvDetailStatus(ReceiveAgainstPoViewModel model)
        {

            DataTable receiveAgainstPos = _materialReceiveAgainstPoManager.GetAccessoriesRcvDetailStatus(model.FromDate, model.ToDate, PortalContext.CurrentUser.CompId);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
         
            if (model.FromDate == null && model.ToDate == null)
            {
                fromDateParameter = new ReportParameter("FromDate", "ALL");
                toDateParameter = new ReportParameter("ToDate", "ALL");

            }
            else
            {
                fromDateParameter = new ReportParameter("FromDate", model.FromDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                toDateParameter = new ReportParameter("ToDate", model.ToDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AccessoriesReceivedDetailStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AccRcvDtlDSet", receiveAgainstPos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.5, MarginTop = .1, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .1 };
            return ReportExtension.ToFile((ReportType)Convert.ToInt16(model.ReportType), path, reportDataSources, deviceInformation, reportParameters);
        }


        public ActionResult AccessoriesStatus(InventoryReportViewModel model)
        {
            model.Buyers = _omBuyerManager.GetAllBuyers();
            return View(model);

        }

        public ActionResult AccessoriesStatusReport(InventoryReportViewModel model)
        {
            ModelState.Clear();
            DataTable accessoriesStatusDataTable = _materialReceiveAgainstPoManager.GetAccessoriesStatusDataTable(model.OrderStyleRefId, PortalContext.CurrentUser.CompId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "AccessoriesStatusReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("AccessoriesStatusDset", accessoriesStatusDataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 14, PageHeight = 8.50, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile((ReportType)Convert.ToInt16(model.ReportType), path, reportDataSources, deviceInformation);
            
        }

    }
}