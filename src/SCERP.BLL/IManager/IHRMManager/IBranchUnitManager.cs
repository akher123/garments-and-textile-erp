using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IBranchUnitManager
    {
       List<BranchUnit> GetAllBranchUnit(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, BranchUnit model);
       BranchUnit GetBranchUnitById(int branchUnitId);
       bool IsExistBranchUnit(BranchUnit model);
       int EditBranchUnit(BranchUnit model);
       int SaveBranchUnit(BranchUnit model);
       int DeleteBranchUnit(int branchUnitId);
       List<BranchUnit> GetAllBranchUnitBySearchKey(SearchFieldModel searchFieldModel);
       IEnumerable GetBranchUnitByBranchId(int searchByBranchId);
       IEnumerable GetAllPermittedBranchUnitsByBranchId(int branchId);
       List<Common.PermissionModel.UserUnit> GetAllPermittedBranchUnitIdByBranchId(int branchId);
    }
}
