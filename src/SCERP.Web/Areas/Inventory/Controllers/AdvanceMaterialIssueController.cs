using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class AdvanceMaterialIssueController : BaseController
    {
        private readonly IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
        private readonly IPartyManager _partyManager;
        private readonly IProcessManager _processManager;
        public AdvanceMaterialIssueController(IAdvanceMaterialIssueManager advanceMaterialIssueManager, IPartyManager partyManager, IProcessManager processManager)
        {
            _advanceMaterialIssueManager = advanceMaterialIssueManager;
            _partyManager = partyManager;
            _processManager = processManager;
        }
        [AjaxAuthorize(Roles = "advanceissue-1,advanceissue-2,advanceissue-3")]
        public ActionResult Index(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords= 0;
            model.MaterialIssue.IType = (int)ActionType.AccessoriesIssue;
            model.MaterialIssues = _advanceMaterialIssueManager.GetAdvanceMaterialIssue(model.PageIndex, model.sort, model.sortdir, model.FromDate, model.ToDate, model.SearchString, model.MaterialIssue.IType, out totalRecords);
            return View(model);
        }
        [AjaxAuthorize(Roles = "advanceissue-2,advanceissue-3")]
        public ActionResult Edit(AdvanceMaterialIssueViewModel model)
        {
            const string partyType = "P";
            ModelState.Clear();
            if (model.MaterialIssue.AdvanceMaterialIssueId>0)
            {
                model.MaterialIssue =_advanceMaterialIssueManager.GetVwAdvanceMaterialIssueById( model.MaterialIssue.AdvanceMaterialIssueId);
                model.MaterialIssueDetails =_advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(model.MaterialIssue.AdvanceMaterialIssueId).ToDictionary(x=>Convert.ToString(x.AdvanceMaterialIssueDetailId),x=>x);
            }
            else
            {
                model.MaterialIssue.IRefId = _advanceMaterialIssueManager.GetNewRefId((int) StoreType.Fabric); 
            }
            model.Parties = _partyManager.GetParties(partyType);
            model.Processes = _processManager.GetProcess();
            model.MaterialIssue.IRNoteDate = DateTime.Now;
            model.StoreList = Enum.GetValues(typeof(StoreType)).Cast<StoreType>().Select(x => new { StoreId = Convert.ToInt16(x), Name = x }).Where(x=>x.StoreId>1);
            return View(model);
        }

        [AjaxAuthorize(Roles = "advanceissue-2,advanceissue-3")]
        public ActionResult Save(AdvanceMaterialIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                Inventory_AdvanceMaterialIssue materialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssueById(model.MaterialIssue.AdvanceMaterialIssueId);
                materialIssue.AdvanceMaterialIssueId = model.MaterialIssue.AdvanceMaterialIssueId;
                materialIssue.OrderNo = model.MaterialIssue.OrderNo;
                materialIssue.StyleNo = model.MaterialIssue.StyleNo;
                materialIssue.RefPerson = model.MaterialIssue.RefPerson;
                materialIssue.SlipNo = model.MaterialIssue.SlipNo;
                materialIssue.Remarks = model.MaterialIssue.Remarks;
                materialIssue.IRNoteNo = model.MaterialIssue.IRNoteNo;
                materialIssue.IRNoteDate = model.MaterialIssue.IRNoteDate;
                materialIssue.IRefId = model.MaterialIssue.IRefId;
                materialIssue.StoreId = model.MaterialIssue.StoreId;
                materialIssue.IType = (int) ActionType.YarnIssue;
                materialIssue.ProcessRefId = model.MaterialIssue.ProcessRefId;
                materialIssue.PartyId = model.MaterialIssue.PartyId;
                materialIssue.IssuedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                materialIssue.CompId = PortalContext.CurrentUser.CompId;
                materialIssue.Inventory_AdvanceMaterialIssueDetail = model.MaterialIssueDetails.Select(x => x.Value).Select(x => new Inventory_AdvanceMaterialIssueDetail()
                {
                    AdvanceMaterialIssueDetailId = x.AdvanceMaterialIssueDetailId,
                    AdvanceMaterialIssueId = x.AdvanceMaterialIssueId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    ItemId = x.ItemId,
                    IssueQty = x.IssueQty,
                    IssueRate = x.IssueRate
                    ,
                    CompId = PortalContext.CurrentUser.CompId
                }).ToList();
                materialIssue.Amount = materialIssue.Inventory_AdvanceMaterialIssueDetail.Sum(x => x.IssueQty * x.IssueRate);
                 saveIndex = _advanceMaterialIssueManager.SaveAdvanceMaterialIssue(materialIssue);
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Save Failed!");
        }
        [AjaxAuthorize(Roles = "advanceissue-3")]
        public ActionResult Delete(long advanceMaterialIssueId)
        {
            int deleted = 0;
            try
            {
                const int iType = (int)ActionType.YarnIssue;
                deleted = _advanceMaterialIssueManager.DeleteAdvanceMaterialIssue(advanceMaterialIssueId, iType);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
       
            return deleted > 0 ? Reload() : ErrorResult("Delete Failed!");
        }

        public ActionResult AddNewRow(AdvanceMaterialIssueViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.MaterialIssueDetails.Add(model.Key, model.MaterialIssueDetail);
            return PartialView("~/Areas/Inventory/Views/AdvanceMaterialIssue/_AddNewRow.cshtml", model);
        }
        public ActionResult IssueSummaryReport(AdvanceMaterialIssueViewModel model)
        {

            List<VwAdvanceMaterialIssue> advanceMaterialIssue = _advanceMaterialIssueManager.GetAdvanceMaterialIssues(model.FromDate, model.ToDate,model.SearchString, model.MaterialIssue.StoreId);
            ReportParameter fromDateParameter;
            ReportParameter toDateParameter;
            ReportParameter reportTitle = new ReportParameter("ReportTitle", "Issue SUMMARY REPORT");
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
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "IssueSummaryReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportParameters = new List<ReportParameter>() { fromDateParameter, toDateParameter, reportTitle };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("IssueSummaryDSet", advanceMaterialIssue) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, reportParameters);
        }
        public ActionResult IssuePdfReport(long advanceMaterialIssueId)
        {
            List<VwAdvanceMaterialIssueDetail> advanceMaterialIssueDetails = _advanceMaterialIssueManager.GetVwAdvanceMaterialssDtl(advanceMaterialIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "IssueReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("IssueDetailDSet", advanceMaterialIssueDetails) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop =1, MarginLeft = 0.4, MarginRight = 0.2, MarginBottom = 1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
    }
}