using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
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
    public class RejectYarnIssueController : BaseController
    {

        private readonly IRejectYarnIssueManager _rejectYarnIssueManager;
        private readonly IPartyManager _partyManager;
        public RejectYarnIssueController(IRejectYarnIssueManager rejectYarnIssueManager, IPartyManager partyManager)
        {
            _rejectYarnIssueManager = rejectYarnIssueManager;
            _partyManager = partyManager;
        }

        public ActionResult Index(RejectYarnIssueViewModel model)
        {
            int totalRecord = 0;
            model.RejectYarnIssues = _rejectYarnIssueManager.GetRejectYarns(model.SearchString, model.PageIndex, model.sort, model.sortdir, out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }

        public ActionResult Edit(int rejectYarnIssueId)
        {

            RejectYarnIssueViewModel model = new RejectYarnIssueViewModel();
            
            if (rejectYarnIssueId > 0)
            {
                model.RejectYarnIssue = _rejectYarnIssueManager.GetRejectYarnById(rejectYarnIssueId);
                model.RejectYarnDetails = _rejectYarnIssueManager.GetRejectYarnDetailById(rejectYarnIssueId).ToDictionary(x => Convert.ToString(x.MaterialReceiveDetailId), x =>x );
            }
            else
            {
                model.RejectYarnIssue.IssueDate=DateTime.Now;
                model.RejectYarnIssue.RefId = _rejectYarnIssueManager.GetNewId();
                model.RejectYarnDetails = _rejectYarnIssueManager.GetRejectYarnDetailById(rejectYarnIssueId).ToDictionary(x => Convert.ToString(x.MaterialReceiveDetailId),
                    x =>
                    {
                        x.Qty =x.RejectedQty;
                        return x;

                    });
            }
           
            model.Parties = _partyManager.GetParties("P");
            return View(model);
        }

        public ActionResult Save(RejectYarnIssueViewModel model)
        {
            try
            {
             model.RejectYarnIssue.Inventory_RejectYarnIssueDetail =
              model.RejectYarnDetails.Select(x => new Inventory_RejectYarnIssueDetail()
              {
                  MaterialReceiveDetailId = x.Value.MaterialReceiveDetailId,
                  Qty = (double)x.Value.Qty,
                  RejectYarnIssueId = model.RejectYarnIssue.RejectYarnIssueId
              }).ToList();
                var saved = 0;
                saved = model.RejectYarnIssue.RejectYarnIssueId > 0 ? _rejectYarnIssueManager.EditRejectYarn(model.RejectYarnIssue) : _rejectYarnIssueManager.SaveRejectYarn(model.RejectYarnIssue);
                return saved > 0 ? Reload() : ErrorResult("Saved Failed");
            }
            catch (Exception exception)
            {
                
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
          
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult RejectYarnReport(int rejectYarnIssueId)
        {
            DataTable chemicalIssueChallan = _rejectYarnIssueManager.GetRejectYarnReport(rejectYarnIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "RejectYarnIssueReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RjctYarnIssuDSet", chemicalIssueChallan) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .2, MarginLeft =.2, MarginRight =.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

        }
    }
}