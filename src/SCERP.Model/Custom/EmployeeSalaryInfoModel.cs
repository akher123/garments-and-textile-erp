namespace SCERP.Model.Custom
{
    using System;
    
    public partial class EmployeeSalaryInfoModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeDesignation { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? QuitDate { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public string GenderName { get; set; }
        public string ActiveStatus { get; set; }
    }
}
