namespace SCERP.Model.Custom
{
    using System;
    
    public partial class EmployeesForInOutProcessModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGradeName { get; set; }
        public string EmployeeDesignation { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public string MobilePhone { get; set; }
    }
}
