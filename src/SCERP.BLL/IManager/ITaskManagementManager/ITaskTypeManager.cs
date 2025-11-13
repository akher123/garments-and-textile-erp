using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface ITaskTypeManager
    {
       List<TmTaskType> GetAllTaskType();
       List<TmTaskType> GetAllTaskTypeByPaging(TmTaskType model, out int totalRecords);
       TmTaskType GetTaskTypeByTaskTypeId(int taskTypeId);
       bool IsTaskTypeExist(TmTaskType model);
       int EditTaskType(TmTaskType model);
       int SaveTaskType(TmTaskType model);
       int DeleleTaskType(int taskTypeId);
    }
}
