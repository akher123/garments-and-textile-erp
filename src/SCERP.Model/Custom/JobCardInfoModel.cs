using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class JobCardInfoModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Name { get; set; }
        public string EmployeeCardId { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeDesignation { get; set; }
        public string MobileNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? QuitDate { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<int> PresentDays { get; set; }
        public Nullable<int> LateDays { get; set; }
        public Nullable<int> OSDDays { get; set; }
        public Nullable<int> AbsentDays { get; set; }
        public Nullable<int> LeaveDays { get; set; }
        public Nullable<int> Holidays { get; set; }
        public Nullable<int> WeekendDays { get; set; }
        public Nullable<int> LWP { get; set; }
        public Nullable<int> TotalDays { get; set; }
        public Nullable<int> WorkingDays { get; set; }
        public Nullable<int> PayDays { get; set; }
        public Nullable<decimal> OTHourLast { get; set; }
        public Nullable<decimal> TotalExtraOTHours { get; set; }
        public Nullable<decimal> TotalWeekendOTHours { get; set; }
        public Nullable<decimal> TotalHolidayOTHours { get; set; }
        public DateTime Date { get; set; }
        public string DayName { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public Nullable<int> Delay { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> ExtraOTHours { get; set; }
        public Nullable<decimal> WeekendOTHours { get; set; }
        public Nullable<decimal> HolidayOTHours { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public string Remarks { get; set; }        
        public Nullable<DateTime> LastIncrementDate { get; set; }
        public string SkillType { get; set; }
        public Nullable<decimal> TotalPenaltyOTHours { get; set; }
        public Nullable<int> TotalPenaltyAttendanceDays { get; set; }
        public Nullable<int> TotalPenaltyLeaveDays { get; set; }
        public Nullable<decimal> TotalPenaltyFinancialAmount { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}