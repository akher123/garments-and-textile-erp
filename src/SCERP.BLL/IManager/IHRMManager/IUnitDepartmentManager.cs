using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IUnitDepartmentManager
    {
        List<UnitDepartment> GetAllUnitDepartmentsByPaging(int startPage, int pageSize, UnitDepartment unitDepartment, out int totalRecords);
        UnitDepartment GetUnitDepartmentById(int unitDepartmentId);
        bool IsExistUnitDepartment(UnitDepartment unitDepartment);
        int EditUnitDepartment(UnitDepartment unitDepartment);
        int SaveUnitDepartment(UnitDepartment unitDepartment);
        int DeleteUnitDepartment(int unitDepartmentId);
        List<UnitDepartment> GetAllUnitDepartmentsBySearchKey(UnitDepartment model);

        IEnumerable GetAllUnitDepatmeByBranchUnitId(int branchUnitId);

        List<UnitDepartment> GetUnitDepartmentByUnitId(int unitId);

    }
}
