using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IPayrollManager
{
    public interface ISalaryAdvanceManager
    {
        List<SalaryAdvanceView> GetAllSalaryAdvancesByPaging(int startPage, int pageSize, out int totalRecords, SalaryAdvance salaryAdvance, SearchFieldModel model);

        List<SalaryAdvance> GetAllSalaryAdvances();

        SalaryAdvance GetSalaryAdvanceById(int? id);

        int SaveSalaryAdvance(SalaryAdvance salaryAdvance);

        int EditSalaryAdvance(SalaryAdvance salaryAdvance);

        int DeleteSalaryAdvance(SalaryAdvance salaryAdvance);

        List<SalaryAdvance> GetSalaryAdvanceBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);

        List<EmployeesForAdvanceSalaryCustomModel> GetEmployeesForAdvanceSalary(SalaryAdvance model,
            SearchFieldModel searchFieldModel);

        int SaveEmployeeSalaryAdvance(List<SalaryAdvance> salaryAdvances);
    }
}
