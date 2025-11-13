namespace SCERP.Model.Custom
{
    using System;
    
    public partial class EmployeeLeaveDetailModel
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
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeDesignation { get; set; }
        public string LeaveTitle { get; set; }
        public Nullable<System.DateTime> LeaveCosumedDate { get; set; }
        public Nullable<System.DateTime> ApplicationSubmitDate { get; set; }
        public string LeavePurpose { get; set; }
        public string AddressDuringLeave { get; set; }
        public Nullable<System.DateTime> RecommendationDate { get; set; }
        public string RecommendationPerson { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public string ApprovalPerson { get; set; }
    }
}
