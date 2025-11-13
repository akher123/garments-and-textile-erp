using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface ICompConsumptionRepository:IRepository<OM_CompConsumption>
   {
       IQueryable<VCompConsumption> GetVCompConsumptions(Expression<Func<VCompConsumption, bool>> predicate);
       IQueryable<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(Expression<Func<VwCompConsumptionOrderStyle, bool>> predicate);
       IQueryable<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(string compId, Guid? employeeId,string serchString);
   }
}
