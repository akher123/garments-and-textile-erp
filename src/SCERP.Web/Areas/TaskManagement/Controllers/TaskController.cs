using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.Model.TaskManagementModel;
using SCERP.Web.Areas.TaskManagement.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.TaskManagement.Controllers
{
    public class TaskController : BaseController
    {
         private readonly ITaskManager _taskManager;
        private readonly ISubjectManager _subjectManager;
        private readonly ITmModuleManager _tmModuleManager;
        private readonly IAssigneeManager _assigneeManager;
        private readonly ITaskStatusManager _taskStatusManager;
        private readonly ITaskTypeManager _taskTypeManager;

        public TaskController(ITaskManager taskManager, ISubjectManager subjectManager, ITmModuleManager tmModuleManager, IAssigneeManager assigneeManager, ITaskStatusManager taskStatusManager, ITaskTypeManager taskTypeManager)
        {
            _taskManager = taskManager;
            _subjectManager = subjectManager;
            _tmModuleManager = tmModuleManager;
            _assigneeManager = assigneeManager;
            _taskStatusManager = taskStatusManager;
            _taskTypeManager = taskTypeManager;
        }
        public ActionResult Index(TaskViewModel model)
        {
            ModelState.Clear();
            try
            {
                var totalRecords = 0;
                model.TaskInformations = _taskManager.GetAllTaskByPaging(model, out totalRecords);
                model.TotalRecords = totalRecords;
                model.Assignees = _assigneeManager.GetAllAssignee();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }
        public ActionResult Save(TaskViewModel model)
        {
            var index = 0;
            try
            {
                    bool exist = _taskManager.IsTaskExist(model.Task);
                    if (!exist)
                    {
                        if (model.Task.TaskId > 0)
                        {
                            index = _taskManager.EditTask(model.Task);
                        }
                        else
                        {
                            index = _taskManager.SaveTask(model.Task); 
                        } 
                    }
                    else
                    {
                        return ErrorResult("Same Information Already Exist ! Please Entry Another One.");
                    }
                
             }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(""+exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Save Task");
        }

        public ActionResult Edit(TaskViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.TaskId > 0)
                {
                    TmTask task = _taskManager.GetTaskByTaskId(model.TaskId);
                    model.Task = task;
                    model.Subjects = _subjectManager.GetALLSubject();
                    if (task.EndDate == null)
                    {
                        model.Task.EndDate = DateTime.Now;
                    }
                }
                else
                {
                    model.Task.TaskNumber = _taskManager.GetNewTaskNumber();
                    model.Task.AssignDate = DateTime.Now;
                }
                model.Modules = _tmModuleManager.GetAllModule();
                model.Assignees = _assigneeManager.GetAllAssignee();
                model.TaskTypes = _taskTypeManager.GetAllTaskType();
                model.TaskStatusList = _taskStatusManager.GetAllTaskStatus();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return View(model);
        }

        public ActionResult Delete(int taskId)
        {
            var index = 0;
            try
            {
                 index = _taskManager.DeleteTask(taskId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Subject");
        }
        public JsonResult GetSubjectsByModelId(int moduleId)
        {
            ModelState.Clear();
            var subjects = _subjectManager.GetSubjectsByModelId(moduleId);
            return Json(subjects, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaskSummaryReport(TaskViewModel model)
        {
            var task = _taskManager.GetTaskSummaryReport(model.SearchString, model.AssigneeId);
            string path = Path.Combine(Server.MapPath("~/Areas/TaskManagement/Reports"), "TaskManagementReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() {new ReportDataSource("DataSet1", task)};
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.3, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation);


        }
	}
}