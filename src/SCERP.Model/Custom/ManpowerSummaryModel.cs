namespace SCERP.Model.Custom
{
    using System;
    
    public partial class ManpowerSummaryModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public Nullable<int> DeptID { get; set; }
        public int SecID { get; set; }
        public int LineID { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string Designation { get; set; }
        public Nullable<int> TotalEmployee { get; set; }
    }
}
