using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class MachingInterruptionController : BaseController
    {
        private readonly IMachingInterruptionManager _machingInterruptionManager;

        public MachingInterruptionController(IMachingInterruptionManager machingInterruptionManager)
        {
            _machingInterruptionManager = machingInterruptionManager;
        }
        [AjaxAuthorize(Roles = "productionremarks-1,productionremarks-2,productionremarks-3")]
        public ActionResult Index(MachingInterruptionViewModel model) 
        {
            ModelState.Clear();
            try
            {
                model.MachingInterruption.InterrupDate = model.MachingInterruption.InterrupDate?? DateTime.Now;
                model.MachingInterruption.ProcessRefId = ProcessCode.SEWING;
                model.MachingInterruptions = _machingInterruptionManager.GetMachingInterruptionByDate(model.MachingInterruption.InterrupDate, model.MachingInterruption.ProcessRefId, PortalContext.CurrentUser.CompId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "productionremarks-2,productionremarks-3")]
        public JsonResult Save(PROD_MachingInterruption model)
        {
            string messae = "";
            model.CompId = PortalContext.CurrentUser.CompId;
            model.CreatedBy = PortalContext.CurrentUser.UserId;
            model.ProcessRefId = ProcessCode.SEWING;
            int index = _machingInterruptionManager.SaveMachingInterruption(model);
            messae = index>0 ? "Saved Successfully" : "Save Failed";
            return Json(messae, JsonRequestBehavior.AllowGet);
        }
	}
}