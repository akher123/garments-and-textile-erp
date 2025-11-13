using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface IAssigneeManager
    {
       List<TmAssignee> GetAllAssignee();
       TmAssignee GetAssigneeByAssigneeId(int assigneeId);
       List<TmAssignee> GetAllAssigneeByPaging(TmAssignee model, out int totalRecords);
       bool IsAssigneeExist(TmAssignee model);
       int EditAssignee(TmAssignee model);
       int SaveAssignee(TmAssignee model);
       int DeleleAssignee(int assigneeId); 
    }
}
