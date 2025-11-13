using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeGradeManager
    {

        EmployeeGrade GetEmployeeGradeDetail(int employeeGradeId);

        EmployeeGrade GetEmployeeGradeById(int? id);

        int SaveEmployeeGrade(EmployeeGrade aEmployeeGrade);

        int EditEmployeeGrade(EmployeeGrade employeeGrade);

        int DeleteEmployeeGrade(EmployeeGrade employeeGrade);

        List<EmployeeGrade> GetAllEmployeeGradesByPaging(int startPage, int pageSize, EmployeeGrade employeeGrade,
            out int totalRecords);

        bool CheckExistingEmployeeGrade(EmployeeGrade employeeGrade);

        List<EmployeeGrade> GetAllEmployeeGradesBySearchKey(string name, int employeeTypeId);

        List<EmployeeGrade> GetEmployeeGradeByEmployeeTypeId(int id);
        List<EmployeeGrade> GetAllEmployeeGrades();

    }
}
