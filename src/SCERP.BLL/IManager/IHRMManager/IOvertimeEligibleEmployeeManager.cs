using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IOvertimeEligibleEmployeeManager
    {
        List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeByPaging(int startPage, int pageSize, out int totalRecords, OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel);
        int SaveOvertimeEligibleEmployee(List<OvertimeEligibleEmployee> assignedOvertimeEligibleEmployees);
        OvertimeEligibleEmployee GetOvertimeEligibleEmployee(int overtimeEligibleEmployeeId);
        OvertimeEligibleEmployee GetOvertimeEligibleEmployeeByEmpIdAndDate(OvertimeEligibleEmployee overtimeEligibleEmployee);
        List<OvertimeEligibleEmployee> GetOvertimeEligibleEmployeeListByEmpIdAndDate(OvertimeEligibleEmployee overtimeEligibleEmployee);
        int EditOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee);
        List<VOvertimeEligibleEmployeeDetail> GetOvertimeEligibleEmployeeBySearchKey(SearchFieldModel searchField);
        IList<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords, OvertimeEligibleEmployee model, SearchFieldModel searchFieldModel);
        int DeleteOvertimeEligibleEmployee(OvertimeEligibleEmployee overtimeEligibleEmployee);
        int SaveNewEmployeeOverTime(Guid employeeId, DateTime joinDate);
    }
}
