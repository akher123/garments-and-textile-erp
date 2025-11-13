using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskRepository _taskRepository;

        public TaskManager(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<vwTmTaskInformation> GetAllTaskByPaging(TmTask model, out int totalRecords)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            IQueryable<vwTmTaskInformation> taskList = _taskRepository.GetvwTmTaskInformation(
                x => (x.AssigneeId == model.AssigneeId || model.AssigneeId == 0)
                    && ((x.ModuleName.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                        || (x.SubjectName.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                        || (x.TaskName.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))
                        || (x.TaskStatus.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString))));

            totalRecords = taskList.Count();
            switch (model.sort)
            {
                case "ModuleName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.ModuleName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "SubjectName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.SubjectName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.SubjectName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


                case "TaskName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.TaskName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.TaskName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "Assignee":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.Assignee)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.Assignee)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "TaskStatus":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.TaskStatus)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.TaskStatus)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "TaskType":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskList = taskList
                                 .OrderByDescending(r => r.TaskType)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskList = taskList
                                 .OrderBy(r => r.TaskType)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    taskList = taskList
                        .OrderByDescending(r => r.TaskId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return taskList.ToList();
        }

        public TmTask GetTaskByTaskId(int taskId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return _taskRepository.Filter(x => x.CompId == compId).Include(x => x.TmModule).Include(x => x.TmSubject).FirstOrDefault(x => x.TaskId == taskId);
        }

        public int SaveTask(TmTask model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _taskRepository.Save(model);
        }

        public int EditTask(TmTask model)
        {
            TmTask task = _taskRepository.FindOne(x => x.TaskId == model.TaskId);
            task.ModuleId = model.ModuleId;
            task.SubjectId = model.SubjectId;
            task.TaskName = model.TaskName;
            task.TaskStatusId = model.TaskStatusId;
            task.TaskTypeId = model.TaskTypeId;
            task.AssigneeId = model.AssigneeId;
            task.AssignDate = model.AssignDate;
            task.EndDate = model.EndDate;
            return _taskRepository.Edit(task);
        }

        public int DeleteTask(int taskId)
        {
            return _taskRepository.Delete(x => x.TaskId == taskId);
        }

        public bool IsTaskExist(TmTask model)
        {
            return _taskRepository.Exists(x => x.ModuleId == model.ModuleId && x.SubjectId == model.SubjectId && x.TaskName == model.TaskName.Trim() && x.AssigneeId==model.AssigneeId && x.TaskStatusId==model.TaskStatusId && x.TaskTypeId==model.TaskTypeId);
        }

        public string GetNewTaskNumber()
        {
            var taskNumber = _taskRepository.All().Max(x => x.TaskNumber);
            return taskNumber.IncrementOne().PadZero(5);
        }

        public List<vwTmTaskInformation> GetTaskSummaryReport(string searchString, int assigneeId)
        {
            IQueryable<vwTmTaskInformation> taskList = _taskRepository.GetvwTmTaskInformation(
                x => (x.AssigneeId == assigneeId || assigneeId == 0)
                    && ((x.ModuleName.Contains(searchString) || String.IsNullOrEmpty(searchString))
                        || (x.SubjectName.Contains(searchString) || String.IsNullOrEmpty(searchString))
                        || (x.TaskName.Contains(searchString) || String.IsNullOrEmpty(searchString))
                        || (x.TaskStatus.Contains(searchString) || String.IsNullOrEmpty(searchString))));
            return taskList.ToList();
        }
    }
}
