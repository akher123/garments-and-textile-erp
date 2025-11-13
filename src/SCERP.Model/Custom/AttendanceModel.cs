using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class AttendanceModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeDesignation { get; set; }
        public string WorkShiftName { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Status { get; set; }
        public string LateTime { get; set; }
        public string LastDayOutTime { get; set; }
        public Nullable<int> TotalContinuousAbsentDays { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> LastDayOTHours { get; set; }
        public Nullable<decimal> ExtraOTHours { get; set; }
        public Nullable<decimal> LastDayExtraOTHours { get; set; }
        public Nullable<decimal> WeekendOTHours { get; set; }
        public Nullable<decimal> HolidayOTHours { get; set; }
        public string Remarks { get; set; }
        public string SignatureOfEmployee { get; set; }
        public DateTime LastPresentDate { get; set; }
    }
}
