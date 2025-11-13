using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IDepartmentLineManager
    {
        List<DepartmentLine> GetDepartmentLine(int startPage, int pageSize, out int totalRecords, DepartmentLine model, SearchFieldModel searchFieldModel);

        List<DepartmentLine> GetDepartmentLineBySearchKey(SearchFieldModel searchFieldModel);
        int DeleteDepartmentLine(int departmentLineId);
        DepartmentLine GetDepartmentLineById(int departmentLineId);
        bool IsExistDepartmentLine(DepartmentLine model);
        int EditranchDepartmentLine(DepartmentLine model);
        int SaveDepartmentLine(DepartmentLine model);
        List<DepartmentLine> GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId);
    }
}
