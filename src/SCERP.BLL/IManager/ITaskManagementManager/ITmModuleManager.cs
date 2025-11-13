using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;

namespace SCERP.BLL.IManager.ITaskManagementManager
{
   public interface ITmModuleManager
    {
       List<TmModule> GetAllModule();
       List<TmModule> GetAllModuleByPaging(TmModule model, out int totalRecords);
       int EditModule(TmModule model);
       int SaveModule(TmModule model);
       TmModule GetModuleByModuleId(int moduleId);
       int DeleleModule(int moduleId);
       bool IsModuleNameExist(TmModule model);
    }
}
