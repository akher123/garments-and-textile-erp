using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;

namespace SCERP.DAL.Repository.Planning
{
    public class ProcessRepository :Repository<PLAN_Process>, IProcessRepository
    {
        public ProcessRepository(SCERPDBContext context) : base(context)
        {
        }
        public string GetProcessById(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CONVERT(VARCHAR(3),ISNULL(max(ProcessRefId),0)+1),3)  AS ProcessRefId  FROM PLAN_Process where CompId={0}",compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }
    }
}
