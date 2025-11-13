using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface ITaskManager
    {
       List<vwTmTaskInformation> GetAllTaskByPaging(TmTask model, out int totalRecords);
       TmTask GetTaskByTaskId(int taskId);
       int SaveTask(TmTask model);
       int EditTask(TmTask model);
       int DeleteTask(int taskId);
       bool IsTaskExist(TmTask model);
       string GetNewTaskNumber();
       List<vwTmTaskInformation> GetTaskSummaryReport(string searchString, int assigneeId);
    }
}
