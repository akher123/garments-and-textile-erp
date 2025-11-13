using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class WeekendBillModel
    {
        public int WeekendBillId { get; set; }
        public string Date { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string SectionName { get; set; }
        public string EmployeeType { get; set; }
        public string Designation { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> Allowance { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
