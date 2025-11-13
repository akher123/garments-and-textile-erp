using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class EmployeeDailyAttendanceViewModel
    {
        public EmployeeDailyAttendanceViewModel()
        {
            Employee = new Employee();
            EmployeeDailyAttendance = new EmployeeDailyAttendance();
            EmployeeCompanyInfo = new EmployeeCompanyInfo();
            WorkShift = new WorkShift();
        }

        public Employee Employee { get; set; }
        public EmployeeDailyAttendance EmployeeDailyAttendance { get; set; }
        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public WorkShift WorkShift { get; set; }
        public int? WorkGroupId { get; set; }
        public int? WorkShiftId { get; set; }
        public int? DepartmentId { get; set; }
        public string FormalOutTime { get; set; }
        public string LeaveStatus { get; set; }

        #region required For FormalOutTime
        //public string FormalOutTime
        //{
        //    get
        //    {
        //        return HasFormalOutTime();
        //    }
        //}

        //private string HasFormalOutTime()
        //{
        //    string outTime = "";
        //    if (EmployeeDailyAttendance.Date.Date.Equals(DateTime.Today.Date))
        //    {

        //        if (EmployeeDailyAttendance.OutTime != TimeSpan.Parse("00:00:00", CultureInfo.InvariantCulture))
        //        {
        //            DateTime time = DateTime.Today.Add(EmployeeDailyAttendance.OutTime);
        //            outTime = time.ToString("hh:mm tt");

        //        }
        //        if (EmployeeDailyAttendance.OutTime == TimeSpan.Parse("00:00:00", CultureInfo.InvariantCulture))
        //        {
        //            if (DateTime.ParseExact(WorkShift.EndTime, "h:mm tt", CultureInfo.InvariantCulture).TimeOfDay <= DateTime.Now.TimeOfDay)
        //            {
        //                DateTime time = DateTime.Today.Add(EmployeeDailyAttendance.FormalOutTime);
        //                outTime = time.ToString("hh:mm tt");
        //            }
        //            else
        //            {
        //                outTime = "";
        //            }
        //        }
        //        return outTime;
        //    }
        //    else
        //    {
        //        DateTime time = DateTime.Today.Add(EmployeeDailyAttendance.FormalOutTime);
        //        outTime = time.ToString("hh:mm tt");
        //    }
        //    return outTime;

        //}
        #endregion
        
    }
}
