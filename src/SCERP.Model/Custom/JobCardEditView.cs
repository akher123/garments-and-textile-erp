using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class JobCardEditView
    {
        public JobCardEditView()
        {
            Inout = new List<EmployeeInOutEditView>();
        }
        public string JobCardName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string CompanyAddress { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string Designation { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public Nullable<int> TotalDays { get; set; }
        public int PresentDays { get; set; }
        public Nullable<int> OSDDays { get; set; }
        public int LateDays { get; set; }
        public int AbsentDays { get; set; }
        public int WeekendDays { get; set; }
        public int Holidays { get; set; }
        public int LeaveDays { get; set; }
        public int LWPDays { get; set; }
        public int WorkingDays { get; set; }
        public int PayDays { get; set; }
        public decimal TotalOTHours { get; set; }
        public decimal TotalExtraOTHours { get; set; }
        public Nullable<decimal> TotalWeekendOTHours { get; set; }
        public Nullable<decimal> TotalHolidayOTHours { get; set; }
        public Nullable<decimal> TotalPenaltyOTHours { get; set; }
        public Nullable<int> TotalPenaltyAttendanceDays { get; set; }
        public Nullable<int> TotalPenaltyLeaveDays { get; set; }
        public Nullable<decimal> TotalPenaltyFinancialAmount { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FromDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ToDate { get; set; }

        public List<EmployeeInOutEditView> Inout { get; set; }
    }
}
