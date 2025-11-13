using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeJobCardViewModel : Employee
    {
        public DateTime? StardDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TodayDate { get; set; }
        public string EmployeeJoinDate { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string Designation { get; set; }
        public string Salary { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int Weekends { get; set; }
        public int Holidays { get; set; }
        public int LeaveDays { get; set; }
        public int LWP { get; set; }
        public int LateDays { get; set; }
        public int ShortLeave { get; set; }
        public int TotalOT { get; set; }
        public int PresentInWeekend { get; set; }

    }
}