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
    public class TaskTypeManager : ITaskTypeManager
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        public TaskTypeManager(ITaskTypeRepository taskTypeRepository)
        {
            _taskTypeRepository = taskTypeRepository;
        }

        public List<TmTaskType> GetAllTaskType()
        {
            return _taskTypeRepository.All().ToList();
        }

        public List<TmTaskType> GetAllTaskTypeByPaging(TmTaskType model, out int totalRecords)
        {
            int index = model.PageIndex;
            int pageSize = AppConfig.PageSize;
            var taskTypeList =
                _taskTypeRepository.Filter(
                    x => (x.TaskType.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)));
            totalRecords = taskTypeList.Count();
            switch (model.sort)
            {
                case "Assignee":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            taskTypeList = taskTypeList
                                 .OrderByDescending(r => r.TaskType)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            taskTypeList = taskTypeList
                                 .OrderBy(r => r.TaskType)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    taskTypeList = taskTypeList
                        .OrderByDescending(r => r.TaskTypeId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return taskTypeList.ToList();
        }

        public TmTaskType GetTaskTypeByTaskTypeId(int taskTypeId)
        {
            return _taskTypeRepository.FindOne(x => x.TaskTypeId == taskTypeId);
        }

        public bool IsTaskTypeExist(TmTaskType model)
        {
            return _taskTypeRepository.Exists(x => x.TaskType == model.TaskType);
        }

        public int EditTaskType(TmTaskType model)
        {
            TmTaskType taskType =
                _taskTypeRepository.FindOne(x => x.TaskTypeId == model.TaskTypeId);

            taskType.TaskType = model.TaskType;
            return _taskTypeRepository.Edit(taskType);
        }

        public int SaveTaskType(TmTaskType model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _taskTypeRepository.Save(model);
        }

        public int DeleleTaskType(int taskTypeId)
        {
            return _taskTypeRepository.Delete(x => x.TaskTypeId == taskTypeId);
        }
    }
}
