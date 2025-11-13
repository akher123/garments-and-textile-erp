using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class MaternityLeaveInfo : SearchModel<MaternityLeaveInfo>
    {
        public int MaternityPaymentId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public Nullable<System.DateTime> LeaveDayStart { get; set; }
        public Nullable<System.DateTime> LeaveDayEnd { get; set; }
        public Nullable<System.DateTime> FirstPaymentDate { get; set; }
        public Nullable<decimal> FirstPaymentAmount { get; set; }
        public Nullable<System.DateTime> SecondPaymentDate { get; set; }
        public Nullable<decimal> SecondPaymentAmount { get; set; }
        public string CompId { get; set; }
    }
}
