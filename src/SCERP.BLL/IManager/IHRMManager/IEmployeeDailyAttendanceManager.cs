using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IEmployeeDailyAttendanceManager
    {
       IList<VEmployeeDailyAttendanceDetail> GetEmployeeDailyAttendanceByPaging(int startPage, int pageSize, out int totalRecords, EmployeeDailyAttendance model, SearchFieldModel searchFieldModel);

       VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId);
     
       int SaveEmployeeDailyAttendance(EmployeeDailyAttendance dailyAttendance);

       EmployeeDailyAttendance GetEmployeeDailyAttendance(int employeeDailyAttendanceId);
       int EditeEmployeeDailyAttendance(EmployeeDailyAttendance dailyAttendance);
       int DeleteEmployeeDailyAttendance(int id);
       bool ImportMachineAttendanceData(DateTime? fromDate, DateTime? toDate);

       int SaveBulkAttendance(EmployeeDailyAttendance dailyAttendance);

       int EditBulkAttendance(EmployeeDailyAttendance dailyAttendance);

       IList<VEmployeeCompanyInfoDetail> GetEmployees(int startPage, int pageSize, out int totalRecords,
           EmployeeDailyAttendance model, SearchFieldModel searchFieldModel);

       int SaveEmployeeDailyAttendances(List<EmployeeDailyAttendance> employeeDailyAttendances);

       int ProcessEmployeeInOut(SearchFieldModel searchFieldModel);
    }
}
