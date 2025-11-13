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
    
    public class FabricReturnController : BaseController
    {
        private readonly IProgramManager _programManager;
        private readonly IFabricReturnManager _fabricReturnManager;
        public FabricReturnController(IProgramManager programManager, IFabricReturnManager fabricReturnManager)
        {
            _programManager = programManager;
            _fabricReturnManager = fabricReturnManager;
        }
          [AjaxAuthorize(Roles = "greyreceive-1,greyreceive-2,greyreceive-3")]
        public ActionResult Index(FabricReturnViewModel model)
        {
            ModelState.Clear();
            int totalRecord = 0;
            
            model.Programs = _programManager.GetVwProgramsPaging(model.SearchString,ProcessType.KNITTING,model.PageIndex, model.FromDate, model.ToDate,out totalRecord);
            model.TotalRecords = totalRecord;
            return View(model);
        }
          [AjaxAuthorize(Roles = "greyreceive-1,greyreceive-2,greyreceive-3")]
        public ActionResult FabricReturnLsit(long programId)
        {
            ModelState.Clear();
            var model=new FabricReturnViewModel
            {
                FabricReturns = _fabricReturnManager.GetFabricReturnByProgramId(programId),
                FabricReturn = {ProgramId = programId}
            };
            return View("~/Areas/Inventory/Views/FabricReturn/_FabricReturnLsit.cshtml", model);
        }
          [AjaxAuthorize(Roles = "greyreceive-2,greyreceive-3")]
        public ActionResult Edit(FabricReturnViewModel model)
        {
            ModelState.Clear();
            if (model.FabricReturn.FabricReturnId>0)
            {
               model.FabricReturn= _fabricReturnManager.GetFabricReturnById(model.FabricReturn.FabricReturnId);
            }
            else
            {
                model.FabricReturn.ReturnDate = DateTime.Now;
            }
            return View(model);
        }
           [AjaxAuthorize(Roles = "greyreceive-2,greyreceive-3")]
        public ActionResult Save(FabricReturnViewModel model)
        {
            int index = 0;
            model.FabricReturn.ReceivedBy = PortalContext.CurrentUser.UserId;
            model.FabricReturn.CompId = PortalContext.CurrentUser.CompId;
            model.FabricReturn.ProcessRefId = ProcessType.KNITTING;
            index = model.FabricReturn.FabricReturnId > 0 ? _fabricReturnManager.EditFabricReturn(model.FabricReturn) : _fabricReturnManager.SaveFabricReturn(model.FabricReturn);

            if (index>0)
            {
                return RedirectToAction("FabricReturnLsit", new { programId = model.FabricReturn.ProgramId});
            }
            else
            {
                return ErrorMessageResult();
            }
        }
           [AjaxAuthorize(Roles = "greyreceive-3")]
        public ActionResult Delete(long programId,long fabricReturnId)
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