using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class AgentRepository :Repository<OM_Agent>, IAgentRepository
    {
        public AgentRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetNewAgentRefId(string compId)
        {
            string sqlQuery =
                String.Format(
                    "SELECT RIGHT('000'+ CAST( ISNULL(MAX(AgentRefId),0)+1 as varchar(3) ),3) as AgentRefId FROM OM_Agent WHERE CompId='{0}'",
                    compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
