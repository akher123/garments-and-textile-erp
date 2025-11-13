using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class HrmAbsentOTPenalty : SearchModel<HrmAbsentOTPenalty>
    {
        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public System.DateTime JoinDate { get; set; }
        public System.Nullable<System.DateTime> Date { get; set; }
        public System.Nullable<decimal> OTDeduction { get; set; }
        public System.Nullable<System.DateTime> CreatedDate { get; set; }
        public System.Nullable<System.Guid> CreatedBy { get; set; }
        public System.Nullable<System.DateTime> EditedDate { get; set; }
        public System.Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
