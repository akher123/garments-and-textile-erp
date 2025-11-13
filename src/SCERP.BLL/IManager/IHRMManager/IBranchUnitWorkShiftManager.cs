using System.Collections;
using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IBranchUnitWorkShiftManager
    {
       List<BranchUnitWorkShift> GetBranchUnitWorkShiftsBuPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, BranchUnitWorkShift model);
       BranchUnitWorkShift GetBranchUnitWorkShiftById(int branchUnitWorkShiftId);
       bool IsExistBranchUnitWorkShift(BranchUnitWorkShift model);
       int EditBranchUnitWorkShift(BranchUnitWorkShift model);
       int SaveBranchUnitWorkShift(BranchUnitWorkShift model);
       int DeleteBranchUnitWorkShift(int branchUnitWorkShiftId);
       List<BranchUnitWorkShift> GetBranchUnitWorkShiftBySearchKey(SearchFieldModel searchFieldModel);
       List<WorkShift> GetBranchUnitWorkShiftByBrancUnitId(int searchByBranchUnitId);
    }
}
