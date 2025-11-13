using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IProcessManager
    {
        List<PLAN_Process> GetProcessByPaging(PLAN_Process model, out int totalRecords);
        PLAN_Process GetProcessById(int processId);
        string GetNewProcessRefId();
        int EditProcess(PLAN_Process model);
        int SaveProcess(PLAN_Process model);
        bool CheckExistingProcess(PLAN_Process model);

        int DeleteProcess(string processRefId);
        List<PLAN_Process> GetProcess();
    }
}
