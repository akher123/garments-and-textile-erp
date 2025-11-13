using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;


namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeManualAttendanceRepository : IRepository<EmployeeDailyAttendance>
    {
        List<string> GetEmployeeData(string employeeCardId, DateTime dt);
        TimeSpan GetFormalOutTime(Guid employeeId);
    }
}
