using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IEmployeeSalaryProcessRepository : IRepository<EmployeeProcessedSalary>
    {
        List<EmployeeProcessedSalary> GetEmployeeSalaryProcessedInfo(int startPage, int pageSize, EmployeeProcessedSalary model, SearchFieldModel searchFieldModel);

        List<EmployeesForSalaryProcessModel> GetEmployeesForSalaryProcess(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model);

        int ProcessBulkEmployeeSalary(SearchFieldModel searchFieldModel, EmployeeProcessedSalary model);
    }
}
