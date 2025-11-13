namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;

    public partial class EmployeeProcessedSalary : SearchModel<EmployeeProcessedSalary>
    {
        public Nullable<long> RowNumber { get; set; }
        public int TotalRows { get; set; }

        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string NameInBengali { get; set; }
        public string MobileNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string MonthName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameInBengali { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddressInBengali { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public string BranchInBengali { get; set; }
        public Nullable<int> BranchUnitId { get; set; }
        public string Unit { get; set; }
        public string UnitInBengali { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string Department { get; set; }
        public string DepartmentInBengali { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Section { get; set; }
        public string SectionInBengali { get; set; }
        public Nullable<int> LineId { get; set; }
        public string Line { get; set; }
        public string LineInBengali { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeTypeInBengali { get; set; }
        public Nullable<int> GradeId { get; set; }
        public string Grade { get; set; }
        public string GradeInBengali { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string Designation { get; set; }
        public string DesignationInBengali { get; set; }
        public Nullable<int> EmployeeCategoryId { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public Nullable<int> TotalDays { get; set; }
        public Nullable<int> WorkingDays { get; set; }
        public Nullable<int> PresentDays { get; set; }
        public Nullable<int> LateDays { get; set; }
        public Nullable<int> OSDDays { get; set; }
        public Nullable<int> AbsentDays { get; set; }
        public Nullable<int> LeaveDays { get; set; }
        public Nullable<int> LWPDays { get; set; }
        public Nullable<int> HolidayDays { get; set; }
        public Nullable<int> WeekendDays { get; set; }
        public Nullable<int> Paydays { get; set; }
        public Nullable<int> CasualLeave { get; set; }
        public Nullable<int> SickLeave { get; set; }
        public Nullable<int> MaternityLeave { get; set; }
        public Nullable<int> EarnLeave { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public Nullable<decimal> LWPFee { get; set; }
        public Nullable<decimal> AbsentFee { get; set; }
        public Nullable<decimal> Advance { get; set; }
        public Nullable<decimal> Stamp { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> AttendanceBonus { get; set; }
        public Nullable<decimal> ShiftingBonus { get; set; }
        public Nullable<decimal> TotalBonus { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> TotalOTAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TotalExtraOTHours { get; set; }
        public Nullable<decimal> TotalExtraOTAmount { get; set; }
        public Nullable<decimal> TotalWeekendOTHours { get; set; }
        public Nullable<decimal> TotalWeekendOTAmount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
