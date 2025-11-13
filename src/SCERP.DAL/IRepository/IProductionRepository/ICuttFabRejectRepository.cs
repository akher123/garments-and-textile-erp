using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ICuttFabRejectRepository:IRepository<PROD_CuttFabReject>
    {
       IQueryable<VwCuttFabReject> GetCuttFabRejects(Expression<Func<VwCuttFabReject, bool>> predicate);
    }
}
