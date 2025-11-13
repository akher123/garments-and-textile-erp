using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeShortLeaveRepository : IRepository<EmployeeShortLeave>
    {
        List<VEmployeeShortLeave> GetAllEmployeeShortLeavesByPaging(int startPage, int pageSize, out int totalRecords, ShortLeaveModel employeeShortLeave);
        EmployeeShortLeave GetEmployeeShortLeaveById(int? id);
        int CheckDuplicateDateTime(EmployeeShortLeave leave);
    }
}
