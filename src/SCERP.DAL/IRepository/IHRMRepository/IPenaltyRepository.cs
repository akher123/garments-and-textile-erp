using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;
using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IPenaltyRepository : IRepository<HrmPenalty>
    {
        IQueryable<VwPenaltyEmployee> GetVwPenaltyEmployee(Expression<Func<VwPenaltyEmployee, bool>> predicates);
        List<SPGetAbsentOtPenaltyEmployee> GetAbsentOtPenaltyEmployee(SearchFieldModel searchFieldModel);
        int SavePenaltyEmployee(List<HrmAbsentOTPenalty> overtimeEligibleEmployees, DateTime fromDate);
    }
}
