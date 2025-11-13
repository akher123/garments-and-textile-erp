using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeTypeRepository : IRepository<EmployeeType>
    {
        EmployeeType GetEmployeeTypeById(int? id);
        List<EmployeeType> GetAllEmployeeTypes();
        List<EmployeeType> GetAllEmployeeTypesByPaging(int startPage, int pageSize, out int totalRecords,
            EmployeeType employeeType);
    }
}
