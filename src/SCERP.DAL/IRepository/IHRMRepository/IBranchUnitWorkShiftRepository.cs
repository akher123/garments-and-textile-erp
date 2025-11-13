using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IBranchUnitWorkShiftRepository : IRepository<BranchUnitWorkShift>
    {
        List<BranchUnitWorkShift> GetBranchUnitWorkShiftsBuPaging
            (int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, BranchUnitWorkShift model);

        BranchUnitWorkShift GetBranchUnitDepartmentById(int branchUnitWorkShiftId);
        List<BranchUnitWorkShift> GetBranchUnitWorkShiftBySearchKey(SearchFieldModel searchFieldModel);
        List<WorkShift> GetBranchUnitWorkShiftByBrancUnitId(int searchByBranchUnitId);
    }
}
