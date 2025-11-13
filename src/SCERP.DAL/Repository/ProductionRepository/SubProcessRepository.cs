using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class SubProcessRepository :Repository<PROD_SubProcess>, ISubProcessRepository
    {
        public SubProcessRepository(SCERPDBContext context) : base(context)
        {
        }

        public string GetSubProcessNewRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CONVERT(VARCHAR(3),ISNULL(max(SubProcessRefId),0)+1),3)  AS SubProcessRefId  FROM PROD_SubProcess where CompId={0}", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

        public IQueryable<VSubProcess> GetVSubProcess(Expression<Func<VSubProcess, bool>> predicates)
        {
            return Context.VSubProcesses.Where(predicates);
        }
    }
}
