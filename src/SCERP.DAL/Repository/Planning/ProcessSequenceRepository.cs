using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class ProcessSequenceRepository :Repository<PLAN_ProcessSequence>, IProcessSequenceRepository
    {
        public ProcessSequenceRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VProcessSequence> GetProcessSequence(string compId, string orderStyleRefId)
        {
            var sqlQuery = "select PS.*, P.ProcessName from " +
                              " PLAN_ProcessSequence as PS inner join PLAN_Process as P on PS.ProcessRefId=P.ProcessRefId and PS.CompId=P.CompId " +
                              "where PS.CompId='{0}' and PS.OrderStyleRefId='{1}' order by PS.ProcessRow";
            sqlQuery = String.Format(sqlQuery, compId, orderStyleRefId);
            return ExecuteQuery(sqlQuery).ToList<VProcessSequence>();
        }
    }
}
