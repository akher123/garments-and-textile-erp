using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class ShortLeaveSummaryModel
    {
        //public Guid EmployeeId { get; set; }
        //public string EmployeeCardId { get; set; }
        //public string Name { get; set; }
        //public string Designation { get; set; }
        //public string Grade { get; set; }
        //public string EmployeeType { get; set; }
        //public string CompanyName { get; set; }
        //public string Branch { get; set; }
        //public string Unit { get; set; }
        //public string Department { get; set; }
        //public string Section { get; set; }
        //public string Line { get; set; }
        //public string JoiningDate { get; set; }
        //public string ShortLeaveDate { get; set; }
        //public int? ReasonType { get; set; }
        //public string ReasonName { get; set; }
        //public TimeSpan FromTime { get; set; }
        //public TimeSpan ToTime { get; set; }
        //public TimeSpan TotalHours { get; set; }
        //public int DepartmentId { get; set; }
        //public int? SectionId { get; set; }
        //public int? LineId { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }


        public Nullable<System.Guid> EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string EmployeeType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string JoiningDate { get; set; }
        public string ShortLeaveDate { get; set; }
        public Nullable<int> ReasonType { get; set; }
        public string ReasonName { get; set; }
        public System.TimeSpan FromTime { get; set; }
        public System.TimeSpan ToTime { get; set; }
        public Nullable<System.TimeSpan> TotalHours { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> LineId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
