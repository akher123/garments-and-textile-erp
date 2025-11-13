using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IAgentManager
    {
        List<OM_Agent> GetAgents();
        List<OM_Agent> GetShepAgents();
        List<OM_Agent> GetAgentByPaging(OM_Agent model, out int totalRecords);
        OM_Agent GetAgentById(int agentId);
        string GetNewAgentRefId();

        int EditAgent(OM_Agent model);
        int SaveAgent(OM_Agent model);
        int DeleteAgent(string agentRefId);

        bool CheckExistingAgent(OM_Agent model);
    }
}
