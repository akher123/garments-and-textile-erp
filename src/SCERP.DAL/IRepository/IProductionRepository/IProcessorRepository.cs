using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IProcessorRepository:IRepository<PROD_Processor>
    {
        string GetProcessorNewRefId(string compId);

        IQueryable<VProcessor> GetVProcessor(Expression<Func<VProcessor,bool>>predicates );
    }
}
