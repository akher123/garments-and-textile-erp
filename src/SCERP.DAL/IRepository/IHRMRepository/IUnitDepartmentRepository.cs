using System.Collections;
using System.Collections.Generic;
using SCERP.Model;
namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IUnitDepartmentRepository : IRepository<UnitDepartment>
    {
        List<UnitDepartment> GetAllUnitDepartmentsByPaging
            (int startPage, int pageSize, UnitDepartment unitDepartment, out int totalRecords);

        IEnumerable GetAllUnitDepatmeByBranchUnitId(int branchUnitId);
    }
}
