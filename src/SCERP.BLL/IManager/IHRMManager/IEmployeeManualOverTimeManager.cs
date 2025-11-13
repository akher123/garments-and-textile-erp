using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;
using SCERP.Model.PayrollModel;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeManualOverTimeManager
    {
        List<EmployeeManualOverTime> GetAllEmployeeManualOverTimeByPaging(EmployeeManualOverTime model, out int totalRecords);
        List<EmployeeManualOverTime> GetAllEmployeeManualOverTimes();
        EmployeeManualOverTime GetEmployeeManualOverTimeById(int id);
        int SaveEmployeeManualOverTime(EmployeeManualOverTime model);
        int EditEmployeeManualOverTime(EmployeeManualOverTime model);
        int DeleteEmployeeManualOverTime(int id);
        bool IsEmployeeManualOverTimeExist(EmployeeManualOverTime model);
        string GetNewEmployeeManualOverTimeRefId(string prifix);
    }
}
