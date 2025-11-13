using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using System;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class EmployeeManualAttendanceRepository : Repository<EmployeeDailyAttendance>, IEmployeeManualAttendanceRepository
    {
        public EmployeeManualAttendanceRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<string> GetEmployeeData(string employeeCardId, DateTime dt)
        {
            var employeeData = new List<string>();

            if (Context.Employees.Count(p => p.EmployeeCardId == employeeCardId) != 1)
            {
                return null;
            }
            return employeeData;
        }

        public TimeSpan GetFormalOutTime(Guid employeeId)
        {
            return new TimeSpan(0,0,0); //Temp
        }
    }
}
