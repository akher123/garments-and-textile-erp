using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IYarnConsumptionRepository:IRepository<OM_YarnConsumption>
    {
        IQueryable<VYarnConsumption> GetVYarnConsumptions(Expression<Func<VYarnConsumption,bool>>predicates);
        string GetNewYCRef(string compId);
       
    }
}
