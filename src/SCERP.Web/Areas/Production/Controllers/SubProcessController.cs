using System;
using System.Web.Mvc;
using Microsoft.Owin.Security.Provider;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class SubProcessController : BaseController
    {
        private readonly ISubProcessManager _subProcessManager;
        public SubProcessController(ISubProcessManager subProcessManager)
        {
            this._subProcessManager = subProcessManager;
        }
        [AjaxAuthorize(Roles = "subprocess-1,subprocess-2,subprocess-3")]
        public ActionResult Index(SubProcessViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.SubProcesses = _subProcessManager.GetSubProcessByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "subprocess-2,subprocess-3")]
        [HttpGet]
        public ActionResult Edit(SubProcessViewModel model)
        {
            ModelState.Clear();
            if (model.SubProcessId > 0)
            {
                var subProcess = _subProcessManager.GetSubProcessById(model.SubProcessId);
                model.SubProcessRefId = subProcess.SubProcessRefId;
                model.ProcessRefId = subProcess.ProcessRefId;// need to check
                model.SubProcessName = subProcess.SubProcessName;
            }
            else
            {
                model.SubProcessRefId = _subProcessManager.GetSubProcessNewRefId();
            }
            model.Processes = _subProcessManager.GetProcessList();
            return View(model);
        }
        [AjaxAuthorize(Roles = "subprocess-2,subprocess-3")]
        [HttpPost]
        public ActionResult Save(PROD_SubProcess model)
        {
            try
            {
                var saveIndex = 0;
                saveIndex = model.SubProcessId > 0 ? _subProcessManager.EditSubProcess(model) : _subProcessManager.SaveSubProcess(model);
                return saveIndex > 0 ? Reload() : ErrorResult("Save Fail !!");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
        }
        [AjaxAuthorize(Roles = "subprocess-3")]
        public ActionResult Delete(SubProcessViewModel model)
        {
            var saveIndex = _subProcessManager.DeleteSubProcess(model.SubProcessRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete SubProcess because of it's all ready used");
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Delate Fail");
        }
        [HttpPost]
        public JsonResult CheckExistingSubProcess(SubProcessViewModel model)
        {
            var isExist = !_subProcessManager.CheckExistingSubProcess(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}