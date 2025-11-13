using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.Hosting;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common.Mail;
using SCERP.Web.Helpers;
using SCERP.Web.Models;
using Spell;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class FinishFabricIssueController : BaseController
    {

        private readonly IFinishFabricIssueManager _finishFabricIssueManager;
        private readonly IPartyManager _partyManager;
        private readonly IBatchManager _batchManager;
        private readonly IFinishFabStoreManager _finishFabStoreManager;
        private readonly IEmailSendManager _emailSendManager;
        public FinishFabricIssueController(IFinishFabStoreManager finishFabStoreManager, IBatchManager batchManager, IPartyManager partyManager, IFinishFabricIssueManager finishFabricIssueManager, IEmailSendManager emailSendManager)
        {
            _partyManager = partyManager;
            _finishFabricIssueManager = finishFabricIssueManager;
            _emailSendManager = emailSendManager;
            _batchManager = batchManager;
            _finishFabStoreManager = finishFabStoreManager;
        }
        [AjaxAuthorize(Roles = "fabricissue-1,fabricissue-2,fabricissue-3")]
        public ActionResult Index(FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.FinishFabricIssues = _finishFabricIssueManager.GetFinishFabIssuresByPaging(model.PageIndex, model.sort, model.sortdir, null,null, model.ToDate, model.FromDate, model.SearchString, PortalContext.CurrentUser.CompId,null, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "fabricissue-2,fabricissue-3")]
        public ActionResult Edit(FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            model.Parties = _partyManager.GetParties("P");
            if (model.FinishFabricIssue.FinishFabIssueId > 0)
            {
                model.FinishFabricIssue = _finishFabricIssueManager.GetFinishFabIssureById(model.FinishFabricIssue.FinishFabIssueId);
                if (model.FinishFabricIssue.IsApproved == true)
                {
                    return
                        ErrorResult("Challan No :" + model.FinishFabricIssue.FinishFabIssureRefId + " is not edit able");
                }
                else
                {
                    List<VwFinishFabricIssueDetail> finishFabricIssueDetails = _finishFabricIssueManager.GetFinishFabIssureDetails(model.FinishFabricIssue.FinishFabIssueId);
                    model.FinishFabricIssueDetails = finishFabricIssueDetails.ToDictionary(x => Convert.ToString(x.FinishFabricIssueDetailId), x => x);
                }

            }
            else
            {
                string compId = PortalContext.CurrentUser.CompId;
                model.FinishFabricIssue.FinishFabIssureRefId = _finishFabricIssueManager.GetFinishFabIssureRefId(compId);
                model.FinishFabricIssue.ChallanDate = DateTime.Now;
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "fabricissue-2,fabricissue-3")]
        public ActionResult Save([Bind(Include = "FinishFabricIssue,FinishFabricIssueDetails")]FinishFabricIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;
                model.FinishFabricIssue.Inventory_FinishFabricIssueDetail = model.FinishFabricIssueDetails
                        .Select(x => x.Value)
                        .Select(x => new Inventory_FinishFabricIssueDetail()
                        {
                            FinishFabricIssueId = model.FinishFabricIssue.FinishFabIssueId,
                            FabQty = x.FabQty,
                            BatchId = x.BatchId,
                            BatchDetailId = x.BatchDetailId,
                            NoOfRoll = x.NoOfRoll,
                            GreyWt = x.GreyWt,
                            CcuffQty = x.CcuffQty,
                            CompId = compId,
                            Remarks = x.Remarks ?? "--"
                        }).ToList();
                if (model.FinishFabricIssue.FinishFabIssueId > 0)
                {
                    model.FinishFabricIssue.EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    saveIndex = _finishFabricIssueManager.EditFinishFabricIssue(model.FinishFabricIssue);
                  
                }
                else
                {
                    model.FinishFabricIssue.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    model.FinishFabricIssue.CompId = compId;
                    model.FinishFabricIssue.IsApproved = false;
                    saveIndex = _finishFabricIssueManager.SaveFinishFabricIssue(model.FinishFabricIssue);
                 
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Save Failed " + exception.Message);
            }
            return saveIndex > 0 ? RedirectToAction("Index") : (ActionResult)ErrorResult("Save Failed ");
        }
        [AjaxAuthorize(Roles = "fabricissue-2,fabricissue-3")]
        public ActionResult AddNewRow([Bind(Include = "FinishFabricIssueDetail")]FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.FinishFabricIssueDetails.Add(model.Key, model.FinishFabricIssueDetail);
            return PartialView("~/Areas/Inventory/Views/FinishFabricIssue/_FabricIssueDetail.cshtml", model);
        }
        [AjaxAuthorize(Roles = "fabricissue-2,fabricissue-3")]
        public ActionResult BatchAutoCompliteByParty(string searchString, long partyId)
        {
            IEnumerable batchList = _batchManager.BatchAutoCompliteByParty(searchString, partyId);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "fabricissue-3")]
        public ActionResult Delete(long finishFabIssueId)
        {
            int delted = 0;
            try
            {
                var inishFabricIssue = _finishFabricIssueManager.GetFinishFabIssureById(finishFabIssueId);
                if (inishFabricIssue.IsApproved == true)
                {
                    return
                        ErrorResult("Challan No :" + inishFabricIssue.FinishFabIssureRefId +" is not permit to Delete");
                }
                else
                {
                    delted = _finishFabricIssueManager.DeleteFinishFabricIssue(finishFabIssueId);
                }

                return delted > 0 ? Reload() : ErrorResult("Delete Failed");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }

        public JsonResult GetBatchDeailsById(long batchId)
        {
            IEnumerable batchDetails = _batchManager.GetBachByBatchId(batchId, PortalContext.CurrentUser.CompId);
            return Json(batchDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchbatchQtyByBatchDetailId(long batchDetailId)
        {
            object finishFabIssues = _finishFabStoreManager.GetFinishFabIssueDetail(batchDetailId);
            return Json(finishFabIssues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinishFabricDeliveryReport(long finishFabIssueId)
        {

            DataTable finishFabricDeliverydt = _finishFabStoreManager.GetFinishFabricDeliveryDataTable(finishFabIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "FinishFabricDeliveryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FinishFabDeliveryDSet", finishFabricDeliverydt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);
            return reportExport;
        }
        public ActionResult FinishFabricIssueReport(long finishFabIssueId)
        {

            DataTable finishFabricDeliverydt = _finishFabStoreManager.GetFinishFabricDeliveryDataTable(finishFabIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "FinishFabricIssueReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FinishFabDeliveryDSet", finishFabricDeliverydt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation);


            return reportExport;

        }

        public ActionResult DyeingBillReport(long finishFabIssueId)
        {

            DataTable finishFabricDeliverydt = _finishFabStoreManager.GetFinishFabricDeliveryDataTable(finishFabIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "DyeingBillReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var totalAmt = Convert.ToDecimal(finishFabricDeliverydt.Compute("sum([" + "Amt" + "])", ""));
            string inWord = SpellAmount.InWrods(Decimal.Round(totalAmt)).Replace("Taka", "");
            var reportParameters = new List<ReportParameter>() { new ReportParameter("InWordAmt", inWord) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BillDSet", finishFabricDeliverydt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            var reportExport= ReportExtension.ToWhiteFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);

            DbEmailModel dbEmail = _emailSendManager.GetDbEmailByTemplateId(EmailTemplateRefId.FABRICSTORE, PortalContext.CurrentUser.CompId);
            dbEmail.Subject = "Finish Fabric Challan Bill Report";
            dbEmail.Body = "Finish Fabric Delivery Bill Report";
            dbEmail.FileAttachments = HostingEnvironment.MapPath(AppConfig.ExportReportFillPath + "." + ReportType.PDF);
             bool send = _emailSendManager.SendDbEmail(dbEmail);

            return reportExport;

        }
        public ActionResult DyeingBillDollars(long finishFabIssueId)
        {

            DataTable finishFabricDeliverydt = _finishFabStoreManager.GetFinishFabricDeliveryDataTable(finishFabIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "DyeingBillReportInDollars.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var totalAmt = Convert.ToDecimal(finishFabricDeliverydt.Compute("sum([" + "Amt" + "])", ""));
            string inWord = SpellAmount.InWrods(Decimal.Round(totalAmt)).Replace("Taka", "dollars");
            var reportParameters = new List<ReportParameter>() { new ReportParameter("InWordAmt", inWord) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("BillDSet", finishFabricDeliverydt) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            var reportExport= ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
            return reportExport;
        }

        public ActionResult FabricChallanApproval(FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.FinishFabricIssues = _finishFabricIssueManager.GetFinishFabIssuresByPaging(model.PageIndex, model.sort, model.sortdir, model.IsSelected,null, model.ToDate, model.FromDate, model.SearchString, PortalContext.CurrentUser.CompId,null, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult SaveApprovedChallan(long finishFabIssueId)
        {
            int approved = _finishFabricIssueManager.ApprovedFabricDeliveryChallan(finishFabIssueId);
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }
    }
}