using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IPenaltyTypeManager
    {
       int EditPenaltyType(HrmPenaltyType model);
       int SavePenaltyType(HrmPenaltyType model);
       List<HrmPenaltyType> GetAllPenaltyTypeByPaging(HrmPenaltyType model, out int totalRecords);
       HrmPenaltyType GetPenaltyTypeByPenaltyTypeId(int penaltyTypeId);
       int DeletePenaltyType(int penaltiTypeId);
       List<HrmPenaltyType> GetAllPenaltyTypes();
       bool IsPenaltyTypeExist(HrmPenaltyType model);
    }
}
