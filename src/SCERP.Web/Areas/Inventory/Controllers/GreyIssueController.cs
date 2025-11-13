using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Inventory.Controllers
{

    public class GreyIssueController : BaseController
    {
        private readonly IGreyIssueManager _greyIssueManager;
        private readonly IPartyManager _partyManager;
        public GreyIssueController(IGreyIssueManager greyIssueManager, IPartyManager partyManager)
        {
            _greyIssueManager = greyIssueManager;
            _partyManager = partyManager;
        }
       [AjaxAuthorize(Roles = "greyissue-1,greyissue-2,greyissue-3")]
        public ActionResult Index(GreyIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.GreyIssues = _greyIssueManager.GetGreyReceiveByPaging(model.FromDate, model.ToDate, model.SearchString, PortalContext.CurrentUser.CompId, model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            return View(model);
        }
        [AjaxAuthorize(Roles = "greyissue-2,greyissue-3")]
        public ActionResult Edit(GreyIssueViewModel model)
        {
            ModelState.Clear();
            Random random = new Random();
            if (model.GreyIssue.GreyIssueId > 0)
            {
                model.GreyIssue = _greyIssueManager.GetGreyissueById(model.GreyIssue.GreyIssueId);
                if (model.GreyIssue.IsApproved)
                {
                    return ErrorResult("This Grey Challan already apporved by stor manage !");
                }
                List<KnittingOrderDelivery> moList = _greyIssueManager.GetKnittingOrderDelivery(0, model.GreyIssue.GreyIssueId);
                
                model.KnittingOrderDelivery = moList.ToDictionary(x => random.Next(100)+ x.ItemCode + x.ColorRefId + x.SizeRefId + x.FinishSizeRefId, x => x);
         
            }
            else
            {
                model.GreyIssue.ChallanDate = DateTime.Now;
                model.GreyIssue.RefId = _greyIssueManager.GetNewRow(PortalContext.CurrentUser.CompId);
            }

            model.Parties = _partyManager.GetParties("P");
            return View(model);
        }
        [AjaxAuthorize(Roles = "greyissue-2,greyissue-3")]
        public ActionResult Save(GreyIssueViewModel model)
        {

            try
            {
                model.GreyIssue.CreatedBy = PortalContext.CurrentUser.UserId;
                model.GreyIssue.CompId = PortalContext.CurrentUser.CompId;
                model.GreyIssue.Posted = POSTED.N.ToString();
                model.GreyIssue.Inventory_GreyIssueDetail =
                model.KnittingOrderDelivery.Select(x => x.Value).Select(x => new Inventory_GreyIssueDetail()
                {
                    GreyIssueId =model.GreyIssue.GreyIssueId, 
                    ProgramRegId = x.ProgramRefId,
                    ItemCode = x.ItemCode,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    FinishSizeRefId = x.FinishSizeRefId,
                    GSM = x.GSM,
                    StLength = x.StLength,
                    Qty = x.Qty,
                    RollQty = x.RollQty,
                }).ToList();

                int saved = model.GreyIssue.GreyIssueId > 0 ? _greyIssueManager.EditGreyIssue(model.GreyIssue) : _greyIssueManager.SaveGreyIssue(model.GreyIssue);
                return saved > 0 ? Reload() : ErrorResult("Save Failed !");
            }
            catch (Exception exception)
            {
                return ErrorResult(exception.Message);
            }
        }
                [AjaxAuthorize(Roles = "greyissue-3")]
        public ActionResult Delete(long greyIssueId)
        {

            try
            {
                var delted = _greyIssueManager.DeleteGreyIssue(greyIssueId);
                return delted > 0 ? Reload() : ErrorResult("Delete Failed");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

        }
        public ActionResult GetKnittingOrderDelivery(int programId)
        {
            Random random = new Random();

            List<KnittingOrderDelivery> knittingOrderDeliveries = _greyIssueManager.GetKnittingOrderDelivery(programId, 0);
            GreyIssueViewModel model = new GreyIssueViewModel();
            model.KnittingOrderDelivery = knittingOrderDeliveries.ToDictionary(x => random.Next(100) + x.ItemCode + x.ColorRefId + x.SizeRefId + x.FinishSizeRefId, x => x);
            return PartialView("~/Areas/Inventory/Views/GreyIssue/_KnittingOrderDelivery.cshtml", model);
        }
        public ActionResult GreyBillReport(long greyIssueId)
        {
            DataTable dataTable = _greyIssueManager.GetGeryIssuePartyChallan(greyIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "GreyChallanBill.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }

          //  var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord), new ReportParameter("ReportTitle", reportTitle) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GreyIssueDset", dataTable)};
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =8.80, PageHeight = 9, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        public ActionResult GreyIssueChallanReport(long greyIssueId)
        {
            DataTable dataTable = _greyIssueManager.GetGeryIssuePartyChallan(greyIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "GreyChallan.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            // var reportParameters = new List<ReportParameter>() { new ReportParameter("InWord", inWord), new ReportParameter("ReportTitle", reportTitle) };
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("GreyIssueDset", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth =9, PageHeight = 9, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = 0.1 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);
        }
        [HttpGet]
        public ActionResult GreyIssueApproval(GreyIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.GreyIssues = _greyIssueManager.GetGreyReceiveByPaging(model.FromDate, model.ToDate, model.SearchString, PortalContext.CurrentUser.CompId, model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        public ActionResult IsGreyIssueApproval(long greyIssueId)
        {
            int index = 0;
            index = _greyIssueManager.GreyIssureApproval(greyIssueId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}