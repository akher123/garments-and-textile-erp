using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IProductionDetailRepository:IRepository<PROD_ProductionDetaill>
    {
        IQueryable<VProductionDetail> GetVProductionDetails(Expression<Func<VProductionDetail, bool>> predicate);
        
    }
}
