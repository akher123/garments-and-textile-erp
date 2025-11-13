using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class AttendanceSummaryModel
    {
        public DateTime TransactionDate { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string Department { get; set; }
        public string SectionName { get; set; }
        public string Section { get; set; }
        public string LineName { get; set; }
        public string Line { get; set; }
        public string EmployeeTypeName { get; set; }
        public string WorkShift { get; set; }
        public Nullable<int> TotalEmployee { get; set; }
        public Nullable<int> TotalPresent { get; set; }
        public Nullable<int> TotalLate { get; set; }
        public Nullable<int> TotalAbsent { get; set; }
        public Nullable<int> TotalLeave { get; set; }
        public Nullable<int> TotalOSD { get; set; }
        public string PercentageOfPresent { get; set; }
    }
}
