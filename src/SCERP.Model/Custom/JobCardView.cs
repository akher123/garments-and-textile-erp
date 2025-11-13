using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class JobCardView
    {
        public string Date { get; set; }
        public string DayName { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public int? Delay { get; set; }
        public decimal? OTHours { get; set; }
        public decimal? ExtraOTHours { get; set; }
        public decimal? WeekendOTHours { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string CardId { get; set; }
        public Guid EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Designation { get; set; }
        public decimal GrossSalary { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string JoiningDate { get; set; }
        public string MobileNo { get; set; }

        public int TotalDays { get; set; }
        public int WorkingDays { get; set; }
        public int PayDays { get; set; }
        public int PresentDays { get; set; }
        public int LateDays { get; set; }
        public int OSDDays { get; set; }
        public int AbsentDays { get; set; }       
        public int WeekendDays { get; set; }
        public int Holidays { get; set; }
        public int LeaveDays { get; set; }
        public int LWP { get; set; }       
        public decimal? OTHourLast { get; set; }
        public decimal? TotalExtraOTHours { get; set; }
        public decimal? TotalWeekendOTHours { get; set; }
        
    }
}
