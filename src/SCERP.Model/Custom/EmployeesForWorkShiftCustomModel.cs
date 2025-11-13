using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Model.Custom
{
    public class EmployeesForWorkShiftCustomModel
    {
        public Nullable<System.Guid> EmployeeId { get; set; }
        public string EmployeeCardNo { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string Grade { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public string WorkGroup { get; set; }
        public Nullable<System.DateTime> WorkGroupAssignedDate { get; set; }
    }
}