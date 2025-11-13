using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IPenaltyManager
    {
       List<VwPenaltyEmployee> GetAllPenaltyByPaging(HrmPenalty model, out int totalRecords);
       int SavePenalty(HrmPenalty model);
       int EditePenalty(HrmPenalty model);
       HrmPenalty GetPenaltyByPenaltyId(int penaltyId);
       int DeletePenalty(int penaltyId);
       bool IsPenaltyExist(HrmPenalty model);
       List<SPGetAbsentOtPenaltyEmployee> GetAbsentOtPenaltyEmployee(SearchFieldModel searchFieldModel);
       int SavePenaltyEmployee(List<HrmAbsentOTPenalty> overtimeEligibleEmployees, DateTime fromDate);
    }
}
