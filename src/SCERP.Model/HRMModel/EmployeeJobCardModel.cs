namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;

    public partial class EmployeeJobCardModel: SearchModel<EmployeeJobCardModel>
    {      
        public Nullable<long> RowNumber { get; set; }
        public int TotalRows { get; set; }

        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameInBengali { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddressInBengali { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchNameInBengali { get; set; }
        public int BranchUnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitNameInBengali { get; set; }
        public int BranchUnitDepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameInBengali { get; set; }
        public int DepartmentSectionId { get; set; }
        public string SectionName { get; set; }
        public string SectionNameInBengali { get; set; }
        public int DepartmentLineId { get; set; }
        public string LineName { get; set; }
        public string LineNameInBengali { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameInBengali { get; set; }
        public string MobileNo { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeTypeInBengali { get; set; }
        public int EmployeeGradeId { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeGradeInBengali { get; set; }
        public int EmployeeDesignationId { get; set; }
        public string EmployeeDesignation { get; set; }
        public string EmployeeDesignationInBengali { get; set; }
        public Nullable<int> EmployeeCategoryId { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public int TotalDays { get; set; }
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int LateDays { get; set; }
        public Nullable<int> OSDDays { get; set; }
        public int AbsentDays { get; set; }
        public int LeaveDays { get; set; }
        public int LWPDays { get; set; }
        public int Holidays { get; set; }
        public int WeekendDays { get; set; }
        public int PayDays { get; set; }
        public Nullable<int> CasualLeave { get; set; }
        public Nullable<int> SickLeave { get; set; }
        public Nullable<int> MaternityLeave { get; set; }
        public Nullable<int> EarnLeave { get; set; }
        public decimal GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public Nullable<decimal> PerDayBasicSalary { get; set; }
        public Nullable<decimal> LWPFee { get; set; }
        public Nullable<decimal> AbsentFee { get; set; }
        public Nullable<decimal> AttendanceBonus { get; set; }
        public Nullable<decimal> ShiftingBonus { get; set; }
        public decimal TotalOTHours { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> EmployeeOTRate { get; set; }
        public Nullable<decimal> TotalOTAmount { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatedBy { get; set; }
        public bool IsActive { get; set; }
    
    }
}
