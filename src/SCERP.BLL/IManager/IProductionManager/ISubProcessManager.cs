using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ISubProcessManager
    {
       List<PLAN_Process> GetProcessList();
       List<PROD_SubProcess> GetSubProcessLsit();
       List<VSubProcess> GetSubProcessByPaging(PROD_SubProcess model, out int totalRecords);

       int SaveSubProcess(PROD_SubProcess subprocess);
        string GetSubProcessNewRefId();
        int EditSubProcess(PROD_SubProcess subprocess);
        bool CheckExistingSubProcess(PROD_SubProcess model);
        PROD_SubProcess GetSubProcessById(long subprocessId);
        int DeleteSubProcess(string subprocessRefId);

       string GetSubProcessNameByRefId(string subProcessRefId);
    }
}
