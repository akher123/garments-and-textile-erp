using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IOvertimeEligibleEmployeeRepository : IRepository<OvertimeEligibleEmployee>
    {
        List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeByPaging(int startPage, int pageSize, out int totalRecords, OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel);
        int SaveOvertimeEligibleEmployee(List<OvertimeEligibleEmployee> overtimeEligibleEmployees);
        List<OvertimeEligibleEmployee> GetOvertimeEligibleEmployeeListBySearchKey(OvertimeEligibleEmployee overtimeEligibleEmployeePar);
        List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeBySearchKey(SearchFieldModel searchField);
        List<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords,
            OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel);

        int DeleteOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee);
    }
}
