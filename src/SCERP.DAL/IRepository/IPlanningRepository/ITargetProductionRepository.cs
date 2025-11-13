using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Model.Planning;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
   public interface ITargetProductionRepository:IRepository<PLAN_TargetProduction>
   {
       IQueryable<VwTargetProduction> VwGetTargetProduction(System.Linq.Expressions.Expression<Func<VwTargetProduction, bool>> predicate);
       List<VwTargetProduction> GetMontylyActiveTargetProductionList(int monthId, int yearId, string compId);
   }
}
