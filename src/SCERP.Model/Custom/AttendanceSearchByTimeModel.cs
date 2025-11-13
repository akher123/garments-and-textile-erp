using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class AttendanceSearchByTimeModel
    {
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string Designation { get; set; }
        public string Date { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
    }
}
