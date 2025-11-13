using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
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
    public class RedyeingFabricIssueController : BaseController
    {
        private readonly IRedyeingFabricIssueManager _redyeingFabricIssueManager;
        private readonly IPartyManager _partyManager;
        public RedyeingFabricIssueController(IRedyeingFabricIssueManager redyeingFabricIssueManager, IPartyManager partyManager)
        {
            _redyeingFabricIssueManager = redyeingFabricIssueManager;
            _partyManager = partyManager;
        }

        [AjaxAuthorize(Roles = "redyeingfabricissue-1,redyeingfabricissue-2,redyeingfabricissue-3")]
        public ActionResult Index(RedyeingFabricIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            var compId = PortalContext.CurrentUser.CompId;
            model.RedyeingFabricIssues = _redyeingFabricIssueManager.GetRedyeingFabIssueByPaging(compId, model.SearchString,  model.PageIndex, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public ActionResult Edit(RedyeingFabricIssueViewModel model)
        {
            ModelState.Clear();
            if (model.RedyeingFabricIssue.RedyeingFabricIssueId > 0)
            {
                model.RedyeingFabricIssue =
                    _redyeingFabricIssueManager.GetRedyeingFabricIssueById(model.RedyeingFabricIssue
                        .RedyeingFabricIssueId);
                if (model.RedyeingFabricIssue.IsApproved)
                {
                    return
                        ErrorResult("Challan No :" + model.RedyeingFabricIssue.RefNo + " is not edit able");
                }
                else
                {
                    List<VwRedyeingFabricIssueDetail> fabricReceiveDetails = _redyeingFabricIssueManager.GetVwRedyeingFabricIssueDetailById(model.RedyeingFabricIssue.RedyeingFabricIssueId);
                    model.RedyeingFabricIssueDetails =
                        fabricReceiveDetails.ToDictionary(x => x.RedyeingFabricIssueDetailId.ToString(), x => x);
                }
         
            }
            else
            {
                model.RedyeingFabricIssue.RefNo = _redyeingFabricIssueManager.GetNewRefNo(PortalContext.CurrentUser.CompId);
                model.RedyeingFabricIssue.ChallanDate = DateTime.Now;
            }
            model.Parties = _partyManager.GetParties("P");
            return View(model);
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public ActionResult Save(RedyeingFabricIssueViewModel model)
        {
            int saveIndex = 0;
            try
            {
                var compId = PortalContext.CurrentUser.CompId;

                model.RedyeingFabricIssue.Inventory_RedyeingFabricIssueDetail = model.RedyeingFabricIssueDetails
                    .Select(x => x.Value)
                    .Select(x => new Inventory_RedyeingFabricIssueDetail()
                    {
                        RedyeingFabricIssueId = model.RedyeingFabricIssue.RedyeingFabricIssueId,
                        FinishQty = x.FinishQty,
                        ReprocessQty = x.ReprocessQty,
                        BatchId = x.BatchId,
                        BatchDetailId = x.BatchDetailId,
                        NoRole = x.NoRole,
                        Remarks = x.Remarks ?? "--"
                    }).ToList();

                if (model.RedyeingFabricIssue.RedyeingFabricIssueId > 0)
                {
                    model.RedyeingFabricIssue.EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    saveIndex = _redyeingFabricIssueManager.EditRedyeingFabricIssue(model.RedyeingFabricIssue);
                }
                else
                {
                    model.RedyeingFabricIssue.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    model.RedyeingFabricIssue.CompId = compId;
                    model.RedyeingFabricIssue.IsApproved = false;
                    saveIndex = _redyeingFabricIssueManager.SaveRedyeingFabricIssue(model.RedyeingFabricIssue);
                }
            }

            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Save Failed " + exception.Message);
            }
            return saveIndex > 0 ? Reload(): ErrorResult("Save Failed! Please Contact with software provider");
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-3")]
        public ActionResult Delete(long redyeingFabricIssueId)
        {
            try
            {
               var redyeingFabricIssue =   _redyeingFabricIssueManager.GetRedyeingFabricIssueById(redyeingFabricIssueId);
                var delted = 0;
                if (redyeingFabricIssue.IsApproved == true)
                {
                    return ErrorResult("Challan No :" + redyeingFabricIssue.RefNo + " is not permit to Delete");
                }
                else
                {
                    delted = _redyeingFabricIssueManager.DeleteRedyeingFabricIssue(redyeingFabricIssueId);
                }

                return delted > 0 ? Reload() : ErrorResult("Delete Failed");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }

        [AjaxAuthorize(Roles = "redyeingfabricissue-3")]
        public ActionResult ApproveRedyeingIssue(long redyeingFabricIssueId)
        {
            int approved = 0;
            if (redyeingFabricIssueId>0)
            {
                approved = _redyeingFabricIssueManager.ApproveRedyeingIssueById(redyeingFabricIssueId);
            }
            else
            {
                return ErrorResult("Invalied Request! Please Contact with software provider!!");
            }
          
            return approved > 0 ? ErrorResult("Approved Successfully") : ErrorResult("Failed To Approved");
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public ActionResult AddNewRow([Bind(Include = "RedyeingFabricIssueDetail")]RedyeingFabricIssueViewModel model)
        {
            ModelState.Clear();
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.RedyeingFabricIssueDetails.Add(model.Key, model.RedyeingFabricIssueDetail);
            return PartialView("~/Areas/Inventory/Views/RedyeingFabricIssue/_RedyeingFabricIssueDetail.cshtml", model);
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public JsonResult GetRedyeingReceivedBatchAutocomplite(string searchString, long partyId)
        {

            object batchList = _redyeingFabricIssueManager.GetRedyeingReceivedBatchAutocomplite(PortalContext.CurrentUser.CompId, searchString, partyId);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }
        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public JsonResult GetRedyeingReceivedBatchDetailByBatchId(long batchId)
        {
            IEnumerable<dynamic> batchdetailList = _redyeingFabricIssueManager.GetRedyeingReceivedBatchDetailByBatchId(batchId);
            string json = JsonConvert.SerializeObject(batchdetailList, Formatting.Indented);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "redyeingfabricissue-2,redyeingfabricissue-3")]
        public ActionResult RedyeingFabricIssueReport(long redyeingFabricIssueId)
        {

            DataTable dataTable = _redyeingFabricIssueManager.GetRedyeingFabricIssue(redyeingFabricIssueId);
            string path = Path.Combine(Server.MapPath("~/Areas/Inventory/Report"), "RedyeingFabricIssueReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("RedyeingFabricIssueDataSet", dataTable) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 8.27, PageHeight = 11.69, MarginTop = .5, MarginLeft = 0.1, MarginRight = 0.1, MarginBottom = .2 };
            var reportExport = ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);

            return reportExport;

        }

	}
}