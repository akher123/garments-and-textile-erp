using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeGradeRepository : IRepository<EmployeeGrade>
    {
        EmployeeGrade GetEmployeeGradeById(int? id);
        List<EmployeeGrade> GetAllEmployeeGradesByPaging(int startPage, int pageSize, out int totalRecords,
            EmployeeGrade employeeGrade);
        List<EmployeeType> GetAllEmployeeType();
        List<EmployeeGrade> GetGradeByEmployeeTypeId(int employeeTypeId);
        List<EmployeeGrade> GetAllEmployeeGradesBySearchKey(string searchByEmployeeGrade, int searchByEmployeeType);
    }
}
