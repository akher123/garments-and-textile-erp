using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface ITaskStatusManager
    {
       List<TmTaskStatus> GetAllTaskStatusByPaging(TmTaskStatus model, out int totalRecords);
       List<TmTaskStatus> GetAllTaskStatus();
       int DeleleTaskStatus(int taskStatusId);
       int SaveTaskStatus(TmTaskStatus model);
       int EditTaskStatus(TmTaskStatus model);
       bool IsTaskStatusExist(TmTaskStatus model);
       TmTaskStatus GetTaskStatusByTaskStatusId(int taskStatusId);
       
    }
}
