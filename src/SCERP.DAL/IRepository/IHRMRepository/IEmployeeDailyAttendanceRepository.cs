using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeDailyAttendanceRepository : IRepository<EmployeeDailyAttendance>
    {

        List<VEmployeeDailyAttendanceDetail> GetEmployeeDailyAttendanceByPaging(int startPage, int pageSize, out int totalRecords, EmployeeDailyAttendance model, SearchFieldModel searchFieldModel);
        EmployeeDailyAttendance GetEmployeeDailyAttendance(int employeeDailyAttendanceId);
        bool ImportMachineAttendanceData(object[] parameters);

        List<VEmployeeCompanyInfoDetail> GetEmployes(int startPage, int pageSize, out int totalRecords,
            EmployeeDailyAttendance model, SearchFieldModel searchFieldModel);

        int SaveEmployeeDailyAttendances(List<EmployeeDailyAttendance> employeeDailyAttendances);

        int ProcessEmployeeInOut(SearchFieldModel searchFieldModel);
    }
}
