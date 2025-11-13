using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeManualAttendanceManager
    {
        List<string> GetEmployeeData(string employeeCardId, DateTime dt);

        string SaveEmployeeManualAttedance(EmployeeDailyAttendance eda);
    }
}
