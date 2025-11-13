namespace SCERP.Model.Custom
{
    using System;
    
    public partial class EmployeeLeaveHistoryModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }

        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeDesignation { get; set; }
        public string GenderName { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public string ActiveStatus { get; set; }      
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public int Year { get; set; }
        public string LeaveTitle { get; set; }
        public int TotalAllowedLeave { get; set; }
        public int TotalConsumedLeave { get; set; }
        public int ToalAvailableLeave { get; set; }
    }
}
