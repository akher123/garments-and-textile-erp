namespace SCERP.Model
{
    using System;
    
    public partial class SPGetSpecificEmployeeCompanyInfo_Result
    {
        public System.Guid EmployeeId { get; set; }
        public int EmployeeCompanyInfoId { get; set; }
        public int BranchUnitId { get; set; }
        public int BranchUnitDepartmentId { get; set; }
        public int DesignationId { get; set; }
        public Nullable<int> DepartmentSectionId { get; set; }
        public Nullable<int> DepartmentLineId { get; set; }
        public Nullable<int> JobTypeId { get; set; }
        public bool IsActive { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public Nullable<int> EmployeeGradeId { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeDesignation { get; set; }
        public string JobType { get; set; }
        public bool IsEligibleForOvertime { get; set; }
        public string PunchCardNo { get; set; }
        public System.DateTime FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
    }
}
