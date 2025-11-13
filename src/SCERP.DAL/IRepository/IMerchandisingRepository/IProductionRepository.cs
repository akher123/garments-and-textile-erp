using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IProductionRepository:IRepository<PROD_Production>
   {
      
       IQueryable<VwProduction> GetVwProductionList(Expression<Func<VwProduction,bool>>predicates );
   }
}
