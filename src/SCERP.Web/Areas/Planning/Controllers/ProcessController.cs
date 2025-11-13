using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model.Planning;
using SCERP.Web.Areas.Planning.Models.ViewModels;
using SCERP.BLL.IManager.IPlanningManager;

namespace SCERP.Web.Areas.Planning.Controllers
{
    public class ProcessController : BasePlanningController
    {
        private readonly IProcessManager _processManager;
        public ProcessController(IProcessManager processManager)
        {
            this._processManager = processManager;
        }
         [AjaxAuthorize(Roles = "process-1,process-2,process-3")]
        public ActionResult Index(ProcessViewModel model)
        {
            ModelState.Clear();
            var totalRecords=0;
            model.Processes = _processManager.GetProcessByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [HttpGet]
        [AjaxAuthorize(Roles = "process-2,process-3")]
        public ActionResult Edit(ProcessViewModel model)
        {
            ModelState.Clear();
            if (model.ProcessId > 0)
            {
                var process = _processManager.GetProcessById(model.ProcessId);
                model.ProcessName = process.ProcessName;
                model.ProcessCode = process.ProcessCode;
                model.BufferDay = process.BufferDay;
            }
            else
            {
                model.ProcessRefId = _processManager.GetNewProcessRefId();
            }
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "process-2,process-3")]
        public ActionResult Save(PLAN_Process model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.ProcessId > 0 ? _processManager.EditProcess(model) : _processManager.SaveProcess(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Process save fail " + errorMessage);
        }
          [AjaxAuthorize(Roles = "process-3")]
        public ActionResult Delete(PLAN_Process model)
        {
            var saveIndex = _processManager.DeleteProcess(model.ProcessRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Process because of it's all ready used in Process Sequence");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingProcess(PLAN_Process model)
        {
            var isExist = !_processManager.CheckExistingProcess(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }


	}
}