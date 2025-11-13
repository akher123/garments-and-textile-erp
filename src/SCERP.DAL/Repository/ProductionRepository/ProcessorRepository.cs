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
    public class ProcessorRepository : Repository<PROD_Processor>, IProcessorRepository
    {
        public ProcessorRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public string GetProcessorNewRefId(string compId)
        {
            var sqlQuery = String.Format(@"SELECT RIGHT('000'+ CONVERT(VARCHAR(3),ISNULL(max(ProcessorRefId),0)+1),3)  AS ProcessorRefId  FROM PROD_Processor where CompId={0}", compId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

        public IQueryable<VProcessor> GetVProcessor(Expression<Func<VProcessor, bool>> predicates)
        {
            return Context.VProcessors.Where(predicates);
        }
    }
}
