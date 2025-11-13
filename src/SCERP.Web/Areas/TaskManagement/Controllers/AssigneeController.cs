using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.Model.TaskManagementModel;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class AssigneeController : BaseController
    {
        private readonly IAssigneeManager _assigneeManager;

        public AssigneeController(IAssigneeManager assigneeManager)
        {
            _assigneeManager = assigneeManager;
        }
        public ActionResult Index(AssigneeViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.Assignees = _assigneeManager.GetAllAssigneeByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(AssigneeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.AssigneeId > 0)
                {
                    TmAssignee assignee = _assigneeManager.GetAssigneeByAssigneeId(model.AssigneeId);
                    model.Assignee = assignee.Assignee;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(""+exception);
            }
            return View(model);
        }
        
        public ActionResult Save(AssigneeViewModel model)
        {
            int index = 0;
            try
            {
                if (_assigneeManager.IsAssigneeExist(model))
                {
                    return ErrorResult("Assignee :" + model.Assignee + " " + "Already Exist ! Please Entry another one");
                }
                if (model.AssigneeId > 0)
                {
                    index = _assigneeManager.EditAssignee(model);
                }
                else
                {
                    TmAssignee assignee = new TmAssignee { Assignee = model.Assignee };
                    index = _assigneeManager.SaveAssignee(assignee);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Assignee!" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save Assignee!");
        }
        public ActionResult Delete(int assigneeId)
        {
            var index = 0;
            try
            {
                index = _assigneeManager.DeleleAssignee(assigneeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Assignee");
        }
       
	}
}