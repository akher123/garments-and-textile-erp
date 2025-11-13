using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface IExcludedEmployeeFromSalaryProcessRepository : IRepository<PayrollExcludedEmployeeFromSalaryProcess>
    {     
        List<PayrollExcludedEmployeeFromSalaryProcess> GetExcludedEmployeeFromSalaryProcessInfo(int startPage,
            int pageSize, PayrollExcludedEmployeeFromSalaryProcess model, SearchFieldModel searchFieldModel,
            out int totalRecords);

        PayrollExcludedEmployeeFromSalaryProcess GetExcludedEmployeeFromSalaryProcessById(
            int excludedEmployeeFromSalaryProcessId);

        int SaveExcludedEmployeeFromSalaryProcess(
           PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess);

        int EditExcludedEmployeeFromSalaryProcess(
            PayrollExcludedEmployeeFromSalaryProcess payrollExcludedEmployeeFromSalaryProcess);

        List<ExludedEmployeeFromSalaryProcessInfoCustomModel> GetEmployeesForExcludingFromSalaryProcess(
            SearchFieldModel searchFieldModel, ExludedEmployeeFromSalaryProcessInfoCustomModel model);

        int ProcessBulkEmployeesForExcludingFromSalaryProcess(SearchFieldModel searchFieldModel,
            ExludedEmployeeFromSalaryProcessInfoCustomModel model);
    }
}
