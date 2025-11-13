using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface IExcludedEmployeeFromSalaryProcessManager
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
