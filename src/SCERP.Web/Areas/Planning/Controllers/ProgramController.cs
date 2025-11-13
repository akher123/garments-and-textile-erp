using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ProgramController :BaseController
    {
        private readonly IProgramManager _programManager;
        private readonly IProcessSequenceManager _processSequenceManager;
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IOmBuyOrdStyleManager _omBuyOrdStyleManager;
        public ProgramController(IProgramManager programManager
            , IProcessSequenceManager processSequenceManager, IBuyerOrderManager buyerOrderManager, IOmBuyOrdStyleManager omBuyOrdStyleManager)
        {
            this._programManager = programManager;
            this._processSequenceManager = processSequenceManager;
            this._buyerOrderManager = buyerOrderManager;
            this._omBuyOrdStyleManager = omBuyOrdStyleManager;
        }

        [AjaxAuthorize(Roles = "processingprogram-1,processingprogram-2,processingprogram-3")]
        public ActionResult Index(ProgramViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string closed = "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(closed,model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
          [AjaxAuthorize(Roles = "processingprogram-2,processingprogram-3")]
        public ActionResult ProgramList(ProgramViewModel model)
        {
            ModelState.Clear();
            model.Programs = _programManager.GetPrograms(model.OrderStyleRefId);
            model.ProcessSequences = _processSequenceManager.GetProcessSequence(model.OrderStyleRefId);
            return PartialView("~/Areas/Planning/Views/Program/_ProgramList.cshtml", model);
        }
         [AjaxAuthorize(Roles = "processingprogram-2,processingprogram-3")]
        public ActionResult Save(ProgramViewModel model)
        {
             model.InPutProgramDetails.AddRange(model.OutPutProgramDetails);
            var exist = _programManager.IsExistProgram(model.OrderStyleRefId, model.ProcessRefId);
            if (!exist && model.InPutProgramDetails.Any())
            {
                var program = new PLAN_Program
                {
                    ExpDate = model.ExpDate,
                    OrderStyleRefId = model.OrderStyleRefId,
                    PrgDate = model.PrgDate,
                    Rmks = model.Rmks,
                    ProcessRefId = model.ProcessRefId,
                    ProgramRefId = model.ProgramRefId
                };

                var saveIndex = model.ProgramId > 0 ? _programManager.EditProgram(program, model.InPutProgramDetails) : _programManager.SaveProgram(program, model.InPutProgramDetails);

                if (saveIndex > 0)
                {
                    return RedirectToAction("ProgramList", new { model.OrderStyleRefId });
                }
                else
                {
                    return ErrorResult("Save Failed !!");
                }
            }
            return ErrorResult("This Program Already Created !!");
        }
        public ActionResult ProcessSequeceList(ProgramViewModel model)
        {
            var proSeqList = _processSequenceManager.GetProcessSequence(model.OrderStyleRefId);
            return Json(proSeqList, JsonRequestBehavior.AllowGet);
        }
         [AjaxAuthorize(Roles = "processingprogram-2,processingprogram-3")]
        public ActionResult Edit(ProgramViewModel model)
        {
            ModelState.Clear();
            model.PrgDate = DateTime.Now;
            model.ExpDate = DateTime.Now;
            if (String.IsNullOrEmpty(model.OrderStyleRefId))
            {
                return ErrorResult("PLEASE SELECT ANY STYLE !!");
            }
            else
            {

            if (model.ProgramId > 0)
            {
                var program = _programManager.GetProgramById(model.ProgramId);
                model.ProgramRefId = program.ProgramRefId;
                model.ProcessRefId = program.ProcessRefId;
                model.ExpDate = program.ExpDate;
                model.PrgDate = program.PrgDate;
                model.OrderStyleRefId = program.OrderStyleRefId;
                model.SearchString = program.OrderStyleRefId;
                model.Rmks = program.Rmks;
                model.InPutProgramDetails = _programManager.GetInPutProgramDetails(model.ProgramId);
                model.OutPutProgramDetails = _programManager.GetOutPutProgramDetails(model.ProgramId);
        
            }
            else
            {
                model.ProgramRefId = _programManager.GetNewProgramRefId("NA",ProcessType.NA); //Beacause This program is not used 
            }
            model.ProcessSequences = _processSequenceManager.GetProcessSequence(model.OrderStyleRefId);

            }
            return View("~/Areas/Planning/Views/Program/_Edit.cshtml", model);
        }
         [AjaxAuthorize(Roles = "processingprogram-2,processingprogram-3")]
        public ActionResult InputOutputProgram(ProgramViewModel model)
        {
            model.InPutProgramDetails = _processSequenceManager.GetInPutProgramDetails(model.OrderStyleRefId, model.ProcessRefId);
            model.OutPutProgramDetails = _processSequenceManager.GetOutPutProgramDetails(model.OrderStyleRefId, model.ProcessRefId);
            return PartialView("~/Areas/Planning/Views/Program/_ProgramInOut.cshtml", model);
        }
         [AjaxAuthorize(Roles = "processingprogram-2,processingprogram-3")]
        public ActionResult BuyerOrderStyleLsit(ProgramViewModel model)
        {
            ModelState.Clear();
            model.BuyOrdStyles = _omBuyOrdStyleManager.GetBuyerOrderStyleByOrderNo(model.OrderNo);
            return PartialView("~/Areas/Planning/Views/Program/_OrderStyleList.cshtml", model);
        }
         [AjaxAuthorize(Roles = "processingprogram-3")]
        public ActionResult Delete(string programRefId,string orderStyleRefId)
        {
            var deleteIndex = _programManager.DeleteProgram(programRefId, orderStyleRefId);

            if (deleteIndex > 0)
            {
                return RedirectToAction("ProgramList", new {OrderStyleRefId= orderStyleRefId });
            }
            return ErrorResult("Delate Fail");
        }
    
    }
}