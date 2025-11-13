using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.Model.TaskManagementModel;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class TaskStatusController : BaseController
    {
        private readonly ITaskStatusManager _taskStatusManager;
        public TaskStatusController(ITaskStatusManager taskStatusManager)
        {
            _taskStatusManager = taskStatusManager;
        }
        public ActionResult Index(TaskStatusViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.TaskStatusList = _taskStatusManager.GetAllTaskStatusByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(TaskStatusViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.TaskStatusId > 0)
                {
                    TmTaskStatus taskStatus = _taskStatusManager.GetTaskStatusByTaskStatusId(model.TaskStatusId);
                    model.TaskStatus = taskStatus.TaskStatus;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult Save(TaskStatusViewModel model)
        {
            int index = 0;
            try
            {
                if (_taskStatusManager.IsTaskStatusExist(model))
                {
                    return ErrorResult("Task Status :" + model.TaskStatus + " " + "Already Exist ! Please Entry Another One");
                }
                if (model.TaskStatusId > 0)
                {
                    index = _taskStatusManager.EditTaskStatus(model);
                }
                else
                {
                    TmTaskStatus taskStatus = new TmTaskStatus { TaskStatus = model.TaskStatus };
                    index = _taskStatusManager.SaveTaskStatus(taskStatus);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Task Status!" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save Task Status!");
        }
        public ActionResult Delete(int taskStatusId)
        {
            var index = 0;
            try
            {
                index = _taskStatusManager.DeleleTaskStatus(taskStatusId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Task Status");
        }

    }
}