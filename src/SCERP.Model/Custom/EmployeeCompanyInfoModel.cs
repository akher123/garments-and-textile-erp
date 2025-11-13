using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.Custom
{
    using System;
    using System.Collections.Generic;

    public partial class EmployeeCompanyInfoModel:EmployeeCompanyInfo
    {
        public long? RowID { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGrade { get; set; }
        public string JobType { get; set; }        
    }
}
