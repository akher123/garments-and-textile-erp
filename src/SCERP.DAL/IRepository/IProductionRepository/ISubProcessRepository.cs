using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface ISubProcessRepository : IRepository<PROD_SubProcess>
    {
        string GetSubProcessNewRefId(string compId);


        IQueryable<VSubProcess> GetVSubProcess(Expression<Func<VSubProcess,bool>>predicates );
    }
}
