using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEmployeeGradeSalaryPercentageManager
    {
       List<EmployeeGradeSalaryPercentage> GetEmployeeGradeSalaryPercentages(int startPage, int pageSize, EmployeeGradeSalaryPercentage model, SearchFieldModel searchFieldModel, out int totalRecords);
       EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentageById(int employeeTypeSalaryPercentageId);
       int EditEmployeeGradeSalaryPercentage(EmployeeGradeSalaryPercentage model);
       int SaveEmployeeGradeSalaryPercentage(EmployeeGradeSalaryPercentage model);
       bool IsEmployeeGradeExist(EmployeeGradeSalaryPercentage model);
       List<EmployeeGradeSalaryPercentage> GetEmployeeGradeSalaryPercentageBySearchKey(SearchFieldModel searchField);
       int DeleteEmployeeGradeSalaryPercentage(int employeeTypeSalaryPercentageId);
       EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(int employeeGradeId, int employeeTypeId);
       object GetEmployeeGradeSalaryPercentange(int employeeGradeId, int employeeTypeId, decimal grossSalary);
    }
}
