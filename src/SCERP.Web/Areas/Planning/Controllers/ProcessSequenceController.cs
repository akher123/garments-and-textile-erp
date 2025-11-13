using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.Manager.PlanningManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ProcessSequenceController : BaseController
    {
        private readonly IProcessManager _processManager;
        private readonly IProcessSequenceManager _processSequenceManager;
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        public ProcessSequenceController(IProcessManager processManager
            , IProcessSequenceManager processSequenceManager, IBuyerOrderManager buyerOrderManager, IOmBuyOrdStyleManager omBuyOrdStyleManager)
        {
            this._processManager = processManager;
            this._processSequenceManager = processSequenceManager;
            this._buyerOrderManager = buyerOrderManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
        }
          [AjaxAuthorize(Roles = "processsequence-1,processsequence-2,processsequence-3")]
        public ActionResult Index(ProcessSequenceViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string closed = "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(closed,model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return PartialView("~/Areas/Planning/Views/ProcessSequence/Index.cshtml", model);
        }


        public ActionResult BuyerOrderStyleLsit(ProcessSequenceViewModel model)
        {
            ModelState.Clear();
            model.BuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return PartialView("~/Areas/Planning/Views/ProcessSequence/_OrderStyleList.cshtml", model);
        }

        public ActionResult ProcessSequenceList(ProcessSequenceViewModel model)
        {
            ModelState.Clear();
            model.ProcessRow = _processSequenceManager.GetProcessRow(model.OrderStyleRefId);
            model.Processes = _processManager.GetProcess();
            model.ProcessSequences = _processSequenceManager.GetProcessSequence(model.OrderStyleRefId);
            return PartialView("~/Areas/Planning/Views/ProcessSequence/_ProcessSequenceList.cshtml", model);
        }
       [AjaxAuthorize(Roles = "processsequence-2,processsequence-3")]
        public ActionResult Edit(ProcessSequenceViewModel model)
        {
            ModelState.Clear();
            model.Processes = _processManager.GetProcess();
            var processSequence = _processSequenceManager.GetProcessSequenceById(model.ProcessSequenceId, model.OrderStyleRefId);
            model.ProcessSequenceId = processSequence.ProcessSequenceId;
            model.ProcessRefId = processSequence.ProcessRefId;
            model.OrderStyleRefId = processSequence.OrderStyleRefId;
            model.ProcessRow = processSequence.ProcessRow;
            return PartialView("~/Areas/Planning/Views/ProcessSequence/_Edit.cshtml", model);
        }
          [AjaxAuthorize(Roles = "processsequence-2,processsequence-3")]
        public ActionResult Save(PLAN_ProcessSequence model)
        {
            var processSequence = _processSequenceManager.GetProcessSequenceById(model.ProcessSequenceId, model.OrderStyleRefId) ?? new PLAN_ProcessSequence();
            processSequence.OrderStyleRefId = model.OrderStyleRefId;
            processSequence.ProcessRefId = model.ProcessRefId;
            processSequence.ProcessRow = model.ProcessRow;
            processSequence.ProcessSequenceId = model.ProcessSequenceId;
            var effrows = _processSequenceManager.SaveProcessSquence(processSequence);
            if (effrows > 0)
            {
                return RedirectToAction("ProcessSequenceList", new { model.OrderStyleRefId });
            }
            return ErrorResult("Save Fail !");
        }
          [AjaxAuthorize(Roles = "processsequence-2,processsequence-3")]
        public ActionResult SaveDefault(PLAN_ProcessSequence model)
        {
            if (!model.OrderStyleRefId.IsNullOrWhiteSpace())
            {
                var effrows = _processSequenceManager.SaveDefaultProcessSquence(model);
                if (effrows > 0)
                {
                    return RedirectToAction("ProcessSequenceList", new { model.OrderStyleRefId });
                }
                else
                {
                    return ErrorResult("Save Fail !");
                }
            }
            return ErrorResult("Plrease Select Style!");

        }
          [AjaxAuthorize(Roles = "processsequence-3")]
        public ActionResult Delete(PLAN_ProcessSequence model)
        {
            var deleteIndex = _processSequenceManager.DeleteProcessSequence(model.ProcessSequenceId, model.OrderStyleRefId);
            if (deleteIndex > 0)
            {
                return RedirectToAction("ProcessSequenceList", new { model.OrderStyleRefId });
            }
            return ErrorResult("Delete Fail !!");
        }
    }
}