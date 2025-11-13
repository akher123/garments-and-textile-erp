using System;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.HRMManager
{
    public class EmployeeManualAttendanceManager : BaseManager, IEmployeeManualAttendanceManager
    {
        private readonly IEmployeeManualAttendanceRepository _employeeManualAttendanceRepository = null;

        public EmployeeManualAttendanceManager(SCERPDBContext context)
        {
            this._employeeManualAttendanceRepository = new EmployeeManualAttendanceRepository(context);
        }

        public List<string> GetEmployeeData(string employeeCardId, DateTime dt)
        {
            return _employeeManualAttendanceRepository.GetEmployeeData(employeeCardId, dt);
        }

        public string SaveEmployeeManualAttedance(EmployeeDailyAttendance eda)
        {
           

            return "";
        }
    }
}
