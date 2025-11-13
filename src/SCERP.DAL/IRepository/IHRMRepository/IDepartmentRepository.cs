using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        Department GetDepartmentById(int? id);
        List<Department> GetAllDepartments();
        List<Department> GetAllDepartmentsByPaging(int startPage, int pageSize, out int totalRecords,
            Department department);
        List<Department> GetAllDepartmentsBySearchKey(string searchKey);
    }
}
