using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeTypeManager
    {
        List<EmployeeType> GetAllEmployeeTypesByPaging(int startPage, int pageSize, out int totalRecords,EmployeeType employeeType);

        List<EmployeeType> GetAllEmployeeTypes();

        EmployeeType GetEmployeeTypeById(int? id);

        bool CheckExistingEmployeeType(EmployeeType employeeType);

        int SaveEmployeeType(EmployeeType aEmployeeType);

        int EditEmployeeType(EmployeeType employeeType);

        int DeleteEmployeeType(EmployeeType employeeType);

        List<EmployeeType> GetEmployeeTypeBySearchKey(string searchKey);

        IEnumerable GetAllPermittedEmployeeTypes();

        List<Common.PermissionModel.UserEmployeeType> GetAllPermittedEmployeeTypeId();
    }
}
