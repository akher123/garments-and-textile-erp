using System.Collections.Generic;
using System.Linq;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IDepartmentManager
    {
        List<Department> GetAllDepartmentsByPaging(int startPage, int pageSize, out int totalRecords,
            Department department);

        List<Department> GetAllDepartments();

        Department GetDepartmentById(int? id);

        int SaveDepartment(Department aDepartment);

        int EditDepartment(Department department);

        int DeleteDepartment(Department department);

        bool CheckExistingDepartment(Department department);

        List<Department> GetDepartmentBySearchKey(string searchKey);
    }
}
