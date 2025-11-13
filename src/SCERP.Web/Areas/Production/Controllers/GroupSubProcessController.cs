using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class GroupSubProcessController : BaseController
    {
        private readonly IGroupSubProcessManager _subProcessManager;

        public GroupSubProcessController(IGroupSubProcessManager subProcessManager)
        {
            _subProcessManager = subProcessManager;
        }
        [AjaxAuthorize(Roles = "finishingsubprocess-1,finishingsubprocess-2,finishingsubprocess-3")]
        public ActionResult Index(GroupSubProcessModel model)
        {
            int totalRecords = 0;
            ModelState.Clear();
            model.GroupSubProcesses = _subProcessManager.GetGroupSubProcessByPaging(model.PageIndex, model.SearchString, PortalContext.CurrentUser.CompId, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "finishingsubprocess-2,finishingsubprocess-3")]
        public ActionResult Save(GroupSubProcessModel model)
        {
            int saved = 0;
            try
            {
                saved = model.GroupSubProcess.GroupSubProcessId > 0 ? _subProcessManager.EditGroupSubProcess(model.GroupSubProcess) : _subProcessManager.SaveGroupSubProcess(model.GroupSubProcess);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);

            }
            return saved ==0 ? ErrorResult("Save Failed") : Reload();

        }

        [AjaxAuthorize(Roles = "finishingsubprocess-2,finishingsubprocess-3")]
        public ActionResult Edit(GroupSubProcessModel model)
        {
            if (model.GroupSubProcess.GroupSubProcessId > 0)
            {
                model.GroupSubProcess = _subProcessManager.GetGroupSubProcessById(model.GroupSubProcess.GroupSubProcessId);
            }

            return View(model);
        }
        [AjaxAuthorize(Roles = "finishingsubprocess-3")]
        public ActionResult Delete(int groupSubProcessId)
        {
            int deleted = 0;
            try
            {
                deleted = _subProcessManager.DeleteGroupSubProcess(groupSubProcessId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);

            }
            return deleted == 0 ? ErrorResult("Delete Failed") : Reload();

        }
    }
}