using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttFabricReceiveController : BaseController
    {
        private readonly IFinishFabricIssueManager _finishFabricIssueManager;
        public CuttFabricReceiveController(IFinishFabricIssueManager finishFabricIssueManager)
        {
            _finishFabricIssueManager = finishFabricIssueManager;
        }

        public ActionResult Index(FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            int plummyPartyId = 1;
            model.FinishFabricIssues = _finishFabricIssueManager.GetFinishFabIssuresByPaging(model.PageIndex, model.sort, model.sortdir, true,model.FinishFabricIssue.IsReceived, model.ToDate, model.FromDate, model.SearchString, PortalContext.CurrentUser.CompId, plummyPartyId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult Edit(FinishFabricIssueViewModel model)
        {
            ModelState.Clear();
            if (model.FinishFabricIssue.FinishFabIssueId > 0)
            {
                 model.FinishFabricIssue = _finishFabricIssueManager.GetFinishFabIssureById(model.FinishFabricIssue.FinishFabIssueId);
                 List<VwFinishFabricIssueDetail> finishFabricIssueDetails = _finishFabricIssueManager.GetFinishFabIssureDetails(model.FinishFabricIssue.FinishFabIssueId);
                 model.FinishFabricIssueDetails = finishFabricIssueDetails.ToDictionary(x => Convert.ToString(x.FinishFabricIssueDetailId), x => x);
            }
            else
            {
                string compId = PortalContext.CurrentUser.CompId;
                model.FinishFabricIssue.FinishFabIssureRefId = _finishFabricIssueManager.GetFinishFabIssureRefId(compId);
                model.FinishFabricIssue.ChallanDate = DateTime.Now;
            }
            return View(model);
        }

        public ActionResult UpdateFabricIssue([Bind(Include = "FinishFabricIssue")]FinishFabricIssueViewModel model)
        {
            try
            {
                int saved = _finishFabricIssueManager.UpdateFabricIssue(model.FinishFabricIssue);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Update Failed " + exception.Message);
            }
            return Reload();
        }

        public ActionResult GetBatchAutoCompliteByParty(string searchString)
        {
            object batchList = _finishFabricIssueManager.ReceivedBatchAutoComplite(searchString);
            return Json(batchList, JsonRequestBehavior.AllowGet);
        }
    }
}