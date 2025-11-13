using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IPayrollRepository
{
    public interface ISalaryAdvanceRepository : IRepository<SalaryAdvance>
    {
        SalaryAdvance GetSalaryAdvanceById(int? id);
        List<SalaryAdvance> GetAllSalaryAdvances();
        List<SalaryAdvanceView> GetAllSalaryAdvancesByPaging(int startPage, int pageSize, out int totalRecords, SalaryAdvance salaryAdvance, SearchFieldModel model);
        List<SalaryAdvance> GetSalaryAdvanceBySearchKey(decimal searchByAmount, DateTime searchByFromDate, DateTime searchByToDate);

        List<EmployeesForAdvanceSalaryCustomModel> GetEmployeesForAdvanceSalary(SalaryAdvance model,
            SearchFieldModel searchFieldModel);

        int SaveEmployeeSalaryAdvance(List<SalaryAdvance> salaryAdvances);
    }
}
