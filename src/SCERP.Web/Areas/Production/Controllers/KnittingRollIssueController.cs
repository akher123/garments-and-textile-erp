using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class KnittingRollIssueController : BaseController
    {
        private readonly IKnittingRollIssueManager _knittingRollIssueManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IOmBuyOrdStyleManager _buyOrdStyle;
        private readonly IProgramManager _programManager;
        public KnittingRollIssueController(IOmBuyOrdStyleManager buyOrdStyle, IKnittingRollIssueManager knittingRollIssueManager, IOmBuyerManager buyerManager, IProgramManager programManager)
        {
            _knittingRollIssueManager = knittingRollIssueManager;
            _buyerManager = buyerManager;
            _programManager = programManager;
            _buyOrdStyle = buyOrdStyle;

        }
        [AjaxAuthorize(Roles = "rollissue-1,rollissue-2,rollissue-3")]
        public ActionResult Index(KnittingRollIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.KnittingRollIssues = _knittingRollIssueManager.GetKnittingRollIssueByPaging(model.PageIndex, model.KnittingRollIssue.OrderStyleRefId, model.sortdir,model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            model.Buyers = _buyerManager.GetAllBuyers();
            return View("/Areas/Production/Views/KnittingRollIssue/Index.cshtml", model);
        }
        [AjaxAuthorize(Roles = "rollissue-2,rollissue-3")]
        public ActionResult Edit(KnittingRollIssueViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
  
            if (model.KnittingRollIssue.KnittingRollIssueId > 0)
            {
                model.KnittingRollIssue=_knittingRollIssueManager.GetKnittingRollIssueById(model.KnittingRollIssue.KnittingRollIssueId);
                if (model.KnittingRollIssue.IsRecived==true)
                {
                    return ErrorResult("Challan Is Received !! So edit permission is disabled");
                }
                else
                {
                    model.Dictionary = _knittingRollIssueManager.GetRollIssueDetailsByKnittingRollIssueId(
                    model.KnittingRollIssue.KnittingRollIssueId).ToDictionary(x => x.RollRefNo + x.KnittingRollId, x => x);
                    model.OrderList = _buyOrdStyle.GetOrderByBuyer(model.KnittingRollIssue.BuyerRefId);
                    model.Styles = _buyOrdStyle.GetBuyerOrderStyleByOrderNo(model.KnittingRollIssue.OrderNo);
                    model.Programs = _programManager.GetPrograms(model.KnittingRollIssue.OrderStyleRefId);
                }
           
            }
            else
            {
                model.KnittingRollIssue.IssueDate = DateTime.Now;
                model.KnittingRollIssue.IssueRefNo = _knittingRollIssueManager.GetNewRefNo(PortalContext.CurrentUser.CompId,null);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "rollissue-2,rollissue-3")]
        public ActionResult Save([Bind(Include = "KnittingRollIssue,RollIssueDetails")]KnittingRollIssueViewModel model)
        {

            model.KnittingRollIssue.PROD_KnittingRollIssueDetail = model.RollIssueDetails.Select(x => x.Value).ToList();
            if (model.KnittingRollIssue.KnittingRollIssueId > 0)
            {

                 model.KnittingRollIssue.Editedby = PortalContext.CurrentUser.UserId;
                 model.KnittingRollIssue.EditedDate = DateTime.Now;
                 model.KnittingRollIssue.Posted = POSTED.N.ToString();
                _knittingRollIssueManager.EditKnittingRollIssue(model.KnittingRollIssue);
            }
            else
            {
                model.KnittingRollIssue.CompId = PortalContext.CurrentUser.CompId;
                model.KnittingRollIssue.CreatedBy = PortalContext.CurrentUser.UserId;
                model.KnittingRollIssue.CreatedDate = DateTime.Now;
                model.KnittingRollIssue.Posted = POSTED.N.ToString();
                model.KnittingRollIssue.Remarks = model.KnittingRollIssue.Remarks;
                model.KnittingRollIssue.IssueRefNo = _knittingRollIssueManager.GetNewRefNo(PortalContext.CurrentUser.CompId,model.KnittingRollIssue.ChallanType);
                _knittingRollIssueManager.SaveKnittingRollIssue(model.KnittingRollIssue);
            }

            return RedirectToAction("Index");
        }
        [AjaxAuthorize(Roles = "rollissue-3")]
        public ActionResult Delete(int knittingRollIssueId)
        {
           var knittingRollIssue =
                _knittingRollIssueManager.GetKnittingRollIssueById(knittingRollIssueId);
           if (knittingRollIssue.IsRecived == true)
            {
                return ErrorResult("Challan Is Received !! So edit permission is disabled");
            }
            else
            {
                int deleted = _knittingRollIssueManager.DeleteKnittingRollIssueById(knittingRollIssueId);
                return deleted > 0 ? Reload() : ErrorResult("Delete Failed !!");
            }
          
        }
        public ActionResult GetKnittingRolls([Bind(Include = "KnittingRollIssue")]KnittingRollIssueViewModel model)
        {
            ModelState.Clear();
            List<VwKnittingRollIssueDetail> knittingRolls = _knittingRollIssueManager.GetKnittingRollsByOrderStyleRefId(model.KnittingRollIssue.ProgramRefId,model.KnittingRollIssue.ChallanType, PortalContext.CurrentUser.CompId);
            if (knittingRolls.Any())
            {
                model.Dictionary = knittingRolls.ToDictionary(x => x.RollRefNo + x.KnittingRollId, x => x);
            }

            return PartialView("/Areas/Production/Views/KnittingRollIssue/_KnittingRollDts.cshtml", model);
        }

        public JsonResult GetProgramByOrderStyleRefId(string orderStyleRefId)
        {
            List<PLAN_Program> programs = _programManager.GetPrograms(orderStyleRefId);
            return Json(programs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GreyChallanReceived(KnittingRollIssueViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.KnittingRollIssues = _knittingRollIssueManager.GetPartyKnittingChallanList(model.PageIndex,model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            model.Buyers = _buyerManager.GetAllBuyers();
            return View(model);
        }

        public ActionResult IsReceivedRollChallan(int knittingRollIssueId)
        {
            int index = 0;
            index = _knittingRollIssueManager.IsReceivedRollChallan(knittingRollIssueId, PortalContext.CurrentUser.CompId);
            return index > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}