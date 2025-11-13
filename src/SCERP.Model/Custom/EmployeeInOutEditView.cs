using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class EmployeeInOutEditView
    {
        public string InOutName { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> BranchUnitId { get; set; }
        public string UnitName { get; set; }
        public Nullable<int> BranchUnitDepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> DepartmentSectionId { get; set; }
        public string SectionName { get; set; }
        public Nullable<int> DepartmentLineId { get; set; }
        public string LineName { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public Nullable<int> EmployeeGradeId { get; set; }
        public string EmployeeGrade { get; set; }
        public Nullable<int> EmployeeDesignationId { get; set; }
        public string EmployeeDesignation { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public Nullable<int> BranchUnitWorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public Nullable<int> EmployeeWorkShiftId { get; set; }
        public string MobileNo { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime TransactionDate { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        public Nullable<System.TimeSpan> LastDayOutTime { get; set; }
        public string Status { get; set; }
        public Nullable<int> LateInMinutes { get; set; }
        public Nullable<int> TotalContinuousAbsentDays { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> LastDayOTHours { get; set; }
        public Nullable<decimal> ExtraOTHours { get; set; }
        public Nullable<decimal> LastDayExtraOTHours { get; set; }
        public Nullable<decimal> WeekendOTHours { get; set; }
        public Nullable<decimal> HolidayOTHours { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
