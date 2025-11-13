using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class SalarySheetView
    {
        public string EmployeeCategory { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Company { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public Nullable<int> BranchUnitId { get; set; }
        public string Unit { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string Department { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Section { get; set; }
        public Nullable<int> LineId { get; set; }
        public string Line { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string CardIdAndName { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string GradeAndDesignation { get; set; }
        public string JoiningDate { get; set; }
        public string QuitDate { get; set; }
        public Nullable<int> TotalDays { get; set; }
        public Nullable<int> WorkingDays { get; set; }
        public Nullable<int> Paydays { get; set; }
        public Nullable<int> WeekendDays { get; set; }
        public Nullable<int> HolidayDays { get; set; }
        public Nullable<int> PresentDays { get; set; }
        public Nullable<int> AbsentDays { get; set; }
        public Nullable<int> LateDays { get; set; }
        public Nullable<int> LeaveDays { get; set; }
        public Nullable<int> LWPDays { get; set; }
        public Nullable<int> CasualLeave { get; set; }
        public Nullable<int> SickLeave { get; set; }
        public Nullable<int> MaternityLeave { get; set; }
        public Nullable<int> EarnLeave { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> LWPFee { get; set; }
        public Nullable<decimal> AbsentFee { get; set; }
        public Nullable<decimal> Advance { get; set; }
        public Nullable<decimal> Stamp { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> AttendanceBonus { get; set; }
        public Nullable<decimal> ShiftingBonus { get; set; }
        public Nullable<decimal> TotalBonus { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> TotalOTAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public string MonthName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Nullable<decimal> AdvancedIncomeTax { get; set; }
    }
}
