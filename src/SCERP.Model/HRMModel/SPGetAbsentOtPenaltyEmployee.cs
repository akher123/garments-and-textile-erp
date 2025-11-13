using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class SPGetAbsentOtPenaltyEmployee
    {
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeDesignation { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public System.Nullable<decimal> GrossSalary { get; set; }
        public System.Nullable<decimal> BasicSalary { get; set; }
        public System.Nullable<decimal> Difference { get; set; }
        public System.Nullable<decimal> OTRate { get; set; }
        public System.Nullable<decimal> PenaltyOTHours { get; set; }
    }
}
