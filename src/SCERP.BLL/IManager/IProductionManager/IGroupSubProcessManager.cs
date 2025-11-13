using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IGroupSubProcessManager
    {
       List<PROD_GroupSubProcess> GetAllGroupSubProcess(string compId);
       List<PROD_GroupSubProcess> GetGroupSubProcessByPaging(int pageIndex, string searchString, string compId, out int totalRecords);
       int SaveGroupSubProcess(PROD_GroupSubProcess groupSubProcess);
       int EditGroupSubProcess(PROD_GroupSubProcess groupSubProcess);
       PROD_GroupSubProcess GetGroupSubProcessById(int groupSubProcessId);
       int DeleteGroupSubProcess(int groupSubProcessId);
    }
}
