using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface ICompConsumptionDetailRepository : IRepository<OM_CompConsumptionDetail>
    {
        IQueryable<VCompConsumptionDetail> GetVCompConsumptionDetails(Expression<Func<VCompConsumptionDetail,bool>>predicate);
     
    }
}
