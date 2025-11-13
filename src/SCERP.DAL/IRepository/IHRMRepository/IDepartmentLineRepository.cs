using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IDepartmentLineRepository : IRepository<DepartmentLine>
    {
        List<DepartmentLine> GetDepartmentLine(int startPage, int pageSize, out int totalRecords, DepartmentLine model, SearchFieldModel searchFieldModel);
        List<DepartmentLine> GetDepartmentLineBySearchKey
            (SearchFieldModel searchFieldModel);

        DepartmentLine GetDepartmentLineById(int departmentLineId);
        List<DepartmentLine> GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId);
    }
}
