using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeGradeSalaryPercentageRepository : IRepository<EmployeeGradeSalaryPercentage>
    {
        List<EmployeeGradeSalaryPercentage> GetEmployeeTypeSalaryPercentages
           (int startPage, int pageSize, EmployeeGradeSalaryPercentage model,SearchFieldModel searchField, out int totalRecords);

        List<EmployeeGradeSalaryPercentage> GetEmployeeTypeSalaryPercentageBySearchKey(SearchFieldModel searchField);
        EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentageById(int employeeGradeSalaryPercentageId);
        EmployeeGradeSalaryPercentage GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(int employeeGradeId, int employeeTypeId);
    }
}
