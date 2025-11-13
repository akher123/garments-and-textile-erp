using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class CollarCuffReceiveController : BaseController
    {

        private readonly IProgramManager _programManager;
        private readonly IFabricReturnManager _fabricReturnManager;

        public CollarCuffReceiveController(IProgramManager programManager, IFabricReturnManager fabricReturnManager)
        {
            _programManager = programManager;
            _fabricReturnManager = fabricReturnManager;
        }
        [AjaxAuthorize(Roles = "collarcuffreceive-1,collarcuffreceive-2,collarcuffreceive-3")]
        public ActionResult Index(FabricReturnViewModel model)
        {
            ModelState.Clear();
            int totalRecord = 0;
            model.Programs = _programManager.GetVwProgramsPaging(model.SearchString, ProcessType.COLLARCUFF, model.PageIndex, model.FromDate, model.ToDate, out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }
        [AjaxAuthorize(Roles = "collarcuffreceive-1,collarcuffreceive-2,collarcuffreceive-3")]
        public ActionResult FabricReturnLsit(long programId)
        {
            ModelState.Clear();
            var model = new FabricReturnViewModel
            {
                FabricReturns = _fabricReturnManager.GetFabricReturnByProgramId(programId),
                FabricReturn = { ProgramId = programId }
            };
            return View("~/Areas/Inventory/Views/CollarCuffReceive/_FabricReturnLsit.cshtml", model);
        }
        [AjaxAuthorize(Roles = "collarcuffreceive-2,collarcuffreceive-3")]
        public ActionResult Edit(FabricReturnViewModel model)
        {
            ModelState.Clear();
            if (model.FabricReturn.FabricReturnId > 0)
            {
                model.FabricReturn = _fabricReturnManager.GetFabricReturnById(model.FabricReturn.FabricReturnId);
                model.ProgramDetailId = model.FabricReturn.ProgramDetailId;
            }
            else
            {
                model.FabricReturn.ReturnDate = DateTime.Now;
            }
            model.ProgramDetails = _programManager.GetOutPutProgramDetails(model.FabricReturn.ProgramId);
            return View(model);
        }
        [AjaxAuthorize(Roles = "collarcuffreceive-2,collarcuffreceive-3")]
        public ActionResult Save(FabricReturnViewModel model)
        {
            int index = 0;
            model.FabricReturn.ReceivedBy = PortalContext.CurrentUser.UserId;
            model.FabricReturn.CompId = PortalContext.CurrentUser.CompId;
            model.FabricReturn.ProcessRefId = ProcessType.COLLARCUFF;
            model.FabricReturn.ProgramDetailId = model.ProgramDetailId;
            index = model.FabricReturn.FabricReturnId > 0 ? _fabricReturnManager.EditFabricReturn(model.FabricReturn) : _fabricReturnManager.SaveFabricReturn(model.FabricReturn);

            if (index > 0)
            {
                return RedirectToAction("FabricReturnLsit", new { programId = model.FabricReturn.ProgramId });
            }
            else
            {
                return ErrorMessageResult();
            }
        }
        [AjaxAuthorize(Roles = "collarcuffreceive-3")]
        public ActionResult Delete(long programId, long fabricReturnId)
        {
            var index = _fabricReturnManager.DeleteFabricById(fabricReturnId);
            if (index > 0)
            {
                return RedirectToAction("FabricReturnLsit", new { programId = programId });
            }
            else
            {
                return ErrorMessageResult();
            }
        }
    }
}