using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IProcessSequenceManager
    {
        List<VProcessSequence> GetProcessSequence(string orderStyleRefId);

        PLAN_ProcessSequence GetProcessSequenceById(long processSequenceId, string orderStyleRefId);
        int SaveProcessSquence(PLAN_ProcessSequence model);

        int DeleteProcessSequence(long processSequenceId, string orderStyleRefId);
        int GetProcessRow(string orderStyleRefId);
        int SaveDefaultProcessSquence(PLAN_ProcessSequence model);
        List<VProgramDetail> GetInPutProgramDetails(string orderStyleRefId, string processRefId);
        List<VProgramDetail> GetOutPutProgramDetails(string orderStyleRefId, string processRefId);
    }
}
