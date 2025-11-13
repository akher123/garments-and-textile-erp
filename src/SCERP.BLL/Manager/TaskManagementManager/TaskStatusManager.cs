using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ITaskManagementManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ITaskManagementRepository;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.Manager.TaskManagementManager
{
    public class TaskStatusManager : ITaskStatusManager
    {
        private readonly ITaskStatusRepository _taskStatusRepository;
        public TaskStatusManager(ITaskStatusRepository taskStatusRepository)
        {
            _taskStatusRepository = taskStatusRepository;
        }

        public List<TmTaskStatus> GetAllTaskStatusByPaging(TmTaskStatus model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;
            var taskStatusList =
                _taskStatusRepository.Filter(
                    x => (x.TaskStatus.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = taskStatusList.Count();
            switch (model.sort)
            {
                case "TaskStatus":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskStatusList = taskStatusList
                                 .OrderByDescending(r => r.TaskStatus)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskStatusList = taskStatusList
                                 .OrderBy(r => r.TaskStatus)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    taskStatusList = taskStatusList
                        .OrderByDescending(r => r.TaskStatusId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return taskStatusList.ToList();
        }

        public List<TmTaskStatus> GetAllTaskStatus()
        {
            return _taskStatusRepository.All().ToList();
        }

        public int DeleleTaskStatus(int taskStatusId)
        {
            return _taskStatusRepository.Delete(x => x.TaskStatusId == taskStatusId);
        }

        public int SaveTaskStatus(TmTaskStatus model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _taskStatusRepository.Save(model);
        }

        public int EditTaskStatus(TmTaskStatus model)
        {
            TmTaskStatus taskStatus =
                _taskStatusRepository.FindOne(x => x.TaskStatusId == model.TaskStatusId);

            taskStatus.TaskStatus = model.TaskStatus;
            return _taskStatusRepository.Edit(taskStatus);
        }

        public bool IsTaskStatusExist(TmTaskStatus model)
        {
            return _taskStatusRepository.Exists(x => x.TaskStatus == model.TaskStatus);
        }

        public TmTaskStatus GetTaskStatusByTaskStatusId(int taskStatusId)
        {
            return _taskStatusRepository.FindOne(x => x.TaskStatusId == taskStatusId);
        }
    }
}
