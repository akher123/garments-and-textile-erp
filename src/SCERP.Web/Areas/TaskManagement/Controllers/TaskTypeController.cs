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
    public class TaskTypeController : BaseController
    {
        private readonly ITaskTypeManager _taskTypeManager;
        public TaskTypeController(ITaskTypeManager taskTypeManager)
        {
            _taskTypeManager = taskTypeManager;
        }
        public ActionResult Index(TaskTypeViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.TaskTypes = _taskTypeManager.GetAllTaskTypeByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        public ActionResult Edit(TaskTypeViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.TaskTypeId > 0)
                {
                    TmTaskType taskType = _taskTypeManager.GetTaskTypeByTaskTypeId(model.TaskTypeId);
                    model.TaskType = taskType.TaskType;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult Save(TaskTypeViewModel model)
        {
            int index = 0;
            try
            {
                if (_taskTypeManager.IsTaskTypeExist(model))
                {
                    return ErrorResult("Task Type :" + model.TaskType + " " + "Already Exist ! Please Entry Another One");
                }
                if (model.TaskTypeId > 0)
                {
                    index = _taskTypeManager.EditTaskType(model);
                }
                else
                {
                    TmTaskType taskType = new TmTaskType { TaskType = model.TaskType };
                    index = _taskTypeManager.SaveTaskType(taskType);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Task Type!" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save Task Type!");
        }
        public ActionResult Delete(int taskTypeId)
        {
            var index = 0;
            try
            {
                index = _taskTypeManager.DeleleTaskType(taskTypeId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Task Type");
        }

    }
}