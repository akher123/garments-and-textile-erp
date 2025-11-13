using System;
using System.Collections.Generic;

namespace SCERP.Model
{
    
    
    public partial class VwSkillMatrixEmployee
    {
        public int SkillMatrixId { get; set; }
        public bool IsActive { get; set; }
        public string CompId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCardId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string Designation { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
    }
}
