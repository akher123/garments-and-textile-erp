using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class YarnReceiveController : BaseController
    {
        private readonly IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
        private readonly IMaterialReceiveAgainstPoManager _materialReceiveAgainstPoManager;
        private readonly ISupplierCompanyManager _supplierCompanyManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly IBookingManager _bookingManager;
        private IProgramManager _programManager;
        private IPurchaseOrderManager _purchaseOrder;

        public YarnReceiveController(IAdvanceMaterialIssueManager advanceMaterialIssueManager, IOmBuyerManager omBuyerManager, IMaterialReceiveAgainstPoManager materialReceiveAgainstPoManager, ISupplierCompanyManager supplierCompanyManager, IBookingManager bookingManager, IProgramManager programManager, IPurchaseOrderManager purchaseOrder)
        {
            _materialReceiveAgainstPoManager = materialReceiveAgainstPoManager;
            _supplierCompanyManager = supplierCompanyManager;
            _omBuyerManager = omBuyerManager;
            _bookingManager = bookingManager;
            _programManager = programManager;
            _purchaseOrder = purchaseOrder;
            _advanceMaterialIssueManager = advanceMaterialIssueManager;
        }
        [AjaxAuthorize(Roles = "yarnReceive-1,yarnReceive-2,yarnReceive-3")]
        public ActionResult Index(ReceiveAgainstPoViewModel model)
        {

            ModelState.Clear();
            string[] types = new[] { RType.BOOKING, RType.RECEIVEWITHOUTBOOKIN, RType.COLLAR_CUTT_PROGRAMWISEYARNRETURN, RType.KNITTING_PROGRAMWISEYARNRETURN, RType.YARNDYED };
            const int yarnStore = (int)StoreType.Yarn;
            int totalRecords;
            model.ReceiveAgainstPos = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByPaging(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, out totalRecords, yarnStore, types);
            model.TotalRecords = totalRecords;
            return View(model);
        }


        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
        public ActionResult Edit(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId > 0)
            {
                var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
                model.ReceiveAgainstPo = receiveAgainstPo;
                model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            }
            else
            {
                model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewRcvRefId();
                model.ReceiveAgainstPo.MRRDate = DateTime.Now;
                model.ReceiveAgainstPo.InvoiceDate = DateTime.Now;
                model.ReceiveAgainstPo.GateEntryDate = DateTime.Now;
                model.ReceiveAgainstPo.ReceiveRegDate = DateTime.Now;
                model.ReceiveAgainstPo.PoDate = DateTime.Now;
            }
            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            return View(model);
        }

        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
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
                    model.ReceiveAgainstPo.StoreId = (int)StoreType.Yarn;
                    model.ReceiveAgainstPo.MRRDate = model.ReceiveAgainstPo.ReceiveRegDate;
                    model.ReceiveAgainstPo.VoucherNo = model.ReceiveAgainstPo.VoucherNo;
                    model.ReceiveAgainstPo.OrderStyleRefId = model.ReceiveAgainstPo.OrderStyleRefId;
                    model.ReceiveAgainstPo.MRRNo = model.ReceiveAgainstPo.ReceiveRegNo;
                    model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail =
                        model.Dictionary.Select(x => x.Value)
                        .Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                        {
                            CompId = compId,
                            ItemId = x.ItemId,
                            ColorRefId = x.ColorRefId,
                            SizeRefId = x.SizeRefId,
                            FColorRefId = x.FColorRefId,
                            ReceivedQty = x.ReceivedQty,
                            ReceivedRate = x.ReceivedRate,
                            LotNo = x.LotNo,
                        }).ToList();
                }
                if (model.ReceiveAgainstPo.MaterialReceiveAgstPoId == 0)
                {
                    model.ReceiveAgainstPo.RefNo = _materialReceiveAgainstPoManager.GetNewRcvRefId();
                }
                savedIndex = _materialReceiveAgainstPoManager.SaveReceiveAgainstPo(model.ReceiveAgainstPo);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return savedIndex > 0 ? Reload() : ErrorResult("Save Failed");
        }

        public ActionResult AddNewRow(ReceiveAgainstPoViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.Dictionary.Add(model.Key, model.PoDetail);
            return View("~/Areas/Inventory/Views/YarnReceive/AddNewRow.cshtml", model);
        }
        [AjaxAuthorize(Roles = "yarnReceive-3")]
        public ActionResult Delete(long materialReceiveAgstPoId)
        {
            int delteIndex = 0;
            try
            {
                const int yarnReceve = (int)ActionType.YarnReceive;
                delteIndex = _materialReceiveAgainstPoManager.DeteteReceiveAgainstPo(materialReceiveAgstPoId, yarnReceve);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return delteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }
        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
        public ActionResult EditQc(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.QcDate = DateTime.Now;
            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
        public ActionResult EditGrn(ReceiveAgainstPoViewModel model)
        {
            var receiveAgainstPo = _materialReceiveAgainstPoManager.GetReceiveAgainstPoByid(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            model.ReceiveAgainstPo = receiveAgainstPo;
            model.ReceiveAgainstPo.GrnDate = DateTime.Now;

            model.Dictionary = _materialReceiveAgainstPoManager.GetDictionary(model.ReceiveAgainstPo.MaterialReceiveAgstPoId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
        public ActionResult UpdateGrn(ReceiveAgainstPoViewModel model)
        {
            int updated = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.ReceiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail = model.Dictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceiveAgainstPoDetail()
                   {
                       CompId = compId,
                       ItemId = x.ItemId,
                       ColorRefId = x.ColorRefId,
                       SizeRefId = x.SizeRefId,
                       ReceivedQty = x.ReceivedQty,
                       ReceivedRate = x.ReceivedRate,
                       RejectedQty = x.RejectedQty ?? 0,
                       MaterialReceiveAgstPoId = x.MaterialReceiveAgstPoId,
                       MaterialReceiveAgstPoDetailId = x.MaterialReceiveAgstPoDetailId
                   }).ToList();
                model.ReceiveAgainstPo.GrnStatus = true;
                const int yarnReceve = (int)ActionType.YarnReceive;
                updated = _materialReceiveAgainstPoManager.UpdateGrn(model.ReceiveAgainstPo, yarnReceve);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);

            }
            return updated > 0 ? Reload() : ErrorResult("Failed to update quality certificat");

        }

        [AjaxAuthorize(Roles = "yarnReceive-2,yarnReceive-3")]
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
                        ItemId = x.ItemId,
                        ColorRefId = x.ColorRefId,
                        SizeRefId = x.SizeRefId,
                        ReceivedQty = x.ReceivedQty,
                        ReceivedRate = x.ReceivedRate,
                        RejectedQty = x.RejectedQty ?? 0,
                        DiscountQty=x.DiscountQty??0,
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
            var booking = new Inventory_Booking();
            const int yarnStore = (int)StoreType.Yarn;
            if (!String.IsNullOrEmpty(model.PiBookingRefId))
            {
                switch (model.RType)
                {
                         
                    case RType.PI:
                        booking = _purchaseOrder.GetPurchaseOrderByRefId(model.PiBookingRefId);
                         model.PiBookings = _purchaseOrder.GetPurchaseOrderByType("Y", booking.OrderStyleRefId);
                        model.Dictionary = _purchaseOrder.GetYarnPurchaseOrderDetails(booking.BookingId);
                             break;
                    case RType.BOOKING:
                        booking = _bookingManager.GetBookingByid(model.PiBookingRefId, yarnStore);
                        model.Dictionary = _bookingManager.GetVwYarnBookingDetail(model.PiBookingRefId);
                        break;
                    case RType.YARNDYED:
                        Inventory_AdvanceMaterialIssue issue = _advanceMaterialIssueManager.GetYarnDeliveryByRefd(model.PiBookingRefId,
                            PortalContext.CurrentUser.CompId);
                        booking.OrderNo = issue.OrderNo;
                        booking.StyleNo = issue.StyleNo;
                        OM_Buyer buyer = _omBuyerManager.GetBuyerByRefId(issue.BuyerRefId, PortalContext.CurrentUser.CompId);
                        booking.BuyerId = buyer.BuyerId;
                        model.Dictionary = _advanceMaterialIssueManager.GetDeliverdYarnDetail(model.PiBookingRefId, PortalContext.CurrentUser.CompId);
                        break;
                    case RType.KNITTING_PROGRAMWISEYARNRETURN:
                        VwProgram program = _programManager.GetProgramByProgramRefId(model.PiBookingRefId, PortalContext.CurrentUser.CompId);
                        buyer = _omBuyerManager.GetBuyerByRefId(program.BuyerRefId, PortalContext.CurrentUser.CompId);
                        booking.OrderNo = program.OrderNo;
                        booking.StyleNo = program.StyleName;
                        booking.BuyerId = buyer.BuyerId;
                        booking.OrderStyleRefId = program.OrderStyleRefId;
                        booking.Remarks = program.RefNo;
                        var programDetails = _programManager.GetProgramYarnReturn(program.ProgramId);
                        model.Dictionary = programDetails.ToDictionary(x => Guid.NewGuid().ToString(), x => new VwMaterialReceiveAgainstPoDetail()
                        {
                            ItemId = x.ItemId,
                            ItemName = x.ItemName,
                            ColorRefId = x.LotRefId,
                            ColorName = x.LotName,
                            SizeName = x.SizeName,
                            FColorRefId = x.ColorRefId,
                            FColorName = x.ColorName,
                            SizeRefId = x.SizeRefId,
                            ReceivedRate =x.IRate,
                            ReceivedQty = x.PgQty,

                        });
                        break;
                    case RType.COLLAR_CUTT_PROGRAMWISEYARNRETURN:
                        VwProgram collarCuffprogram = _programManager.GetProgramByProgramRefId(model.PiBookingRefId, PortalContext.CurrentUser.CompId);
                        buyer = _omBuyerManager.GetBuyerByRefId(collarCuffprogram.BuyerRefId, PortalContext.CurrentUser.CompId);
                        booking.OrderNo = collarCuffprogram.OrderNo;
                        booking.StyleNo = collarCuffprogram.StyleName;
                        booking.BuyerId = buyer.BuyerId;
                        booking.OrderStyleRefId = collarCuffprogram.OrderStyleRefId;
                        booking.Remarks = collarCuffprogram.RefNo;
                        var collarCuttprogramDetails = _programManager.GetProgramYarnReturn(collarCuffprogram.ProgramId);
                        model.Dictionary = collarCuttprogramDetails.ToDictionary(x => Guid.NewGuid().ToString(), x => new VwMaterialReceiveAgainstPoDetail()
                        {
                            ItemId = x.ItemId,
                            ItemName = x.ItemName,
                            ColorRefId = x.LotRefId,
                            ColorName = x.LotName,
                            SizeName = x.SizeName,
                            FColorRefId = x.ColorRefId,
                            FColorName = x.ColorName,
                            SizeRefId = x.SizeRefId,
                            ReceivedRate = x.IRate,
                            ReceivedQty = x.PgQty,

                        });
                        break;
                }
            }
            var rowString = RenderViewToString("~/Areas/Inventory/Views/YarnReceive/AddNewRow.cshtml", model);
            return Json(new { booking = booking, RowString = rowString }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PiBooking(ReceiveAgainstPoViewModel model)
        {
            const int storeId = (int)StoreType.Yarn;
            switch (model.RType)
            {
                case RType.PI:
                    const string pType = "Y";
                    model.PiBookings = _purchaseOrder.GetPurchaseOrderByType(pType, model.OrderStyleRefId);
                    break;
                case RType.BOOKING:
                    model.PiBookings = _bookingManager.GetBooking(storeId);
                    break;
                case RType.YARNDYED:
                    model.PiBookings = _advanceMaterialIssueManager.GetDeliverdYarn(ProcessType.YARNDYEING, storeId);
                    break;
                case RType.KNITTING_PROGRAMWISEYARNRETURN:
                    var programs = _programManager.GetProgramsByProcessType(ProcessType.KNITTING, PortalContext.CurrentUser.CompId);
                    model.PiBookings = programs.Select(x => new { Value = x.ProgramRefId, Text = x.ProgramRefId + " " + x.Party.Name }).ToList();
                    break;
                case RType.COLLAR_CUTT_PROGRAMWISEYARNRETURN:
                    var collarCuffprograms = _programManager.GetProgramsByProcessType(ProcessType.COLLARCUFF, PortalContext.CurrentUser.CompId);
                    model.PiBookings = collarCuffprograms.Select(x => new { Value = x.ProgramRefId, Text = x.ProgramRefId + " " + x.Party.Name }).ToList();
                    break;
            }

            model.SupplierCompanies = _supplierCompanyManager.GetAllSupplierCompany();
            model.ReceiveAgainstPo.PoDate = DateTime.Now;
            return View("~/Areas/Inventory/Views/YarnReceive/_RType.cshtml", model);
        }
        public ActionResult MaterialReceivedAgainstPoOrBookingReport(long materialReceiveAgstPoId, string key)
        {

            List<VwMaterialReceiveAgainstPoDetail> receiveAgainstPoDetails = _materialReceiveAgainstPoManager.GetVwMaterialReceiveAgainstPoDetail(materialReceiveAgstPoId);
            string reportName = "";
            List<ReportDataSource> reportDataSources = new List<ReportDataSource>();
            var deviceInformation = new DeviceInformation();
            switch (key)
            {
                case "GRN":
                    reportName = "YarnGrnReport";
                    reportDataSources = new List<ReportDataSource>() { new ReportDataSource("YarnGrnDSet", receiveAgainstPoDetails) };
                    break;
                case "QC":
                    reportName = "YarnQcReport";
                    reportDataSources = new List<ReportDataSource>() { new ReportDataSource("YarnQcDset", receiveAgainstPoDetails) };
                    deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .5, MarginLeft = 0.5, MarginRight = 0.5, MarginBottom = .5 };
                    break;
                default:
                    reportName = "ReceivedPoOrBookingReport";
                    deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
                    reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RcvAgPoBKDSet", receiveAgainstPoDetails) };
                    break;
            }
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), reportName + ".rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }


            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult MrrSummaryReport(ReceiveAgainstPoViewModel model)
        {

            List<VwMaterialReceiveAgainstPo> receiveAgainstPos = _materialReceiveAgainstPoManager.GetMrrSummaryReport(model.FromDate, model.ToDate, model.SearchString);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
            ReportParameter reportTitlePar = new ReportParameter("ReportTitle", "YARN RECEIVE SUMMARY REPORT");
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
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "MRRSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter, reportTitlePar };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("MRRDSet", receiveAgainstPos) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.3, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }
    }
}