using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Model.Custom
{
    public class EmployeesForAdvanceSalaryCustomModel
    {
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string JoiningDate { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<double> Amount { get; set; }
    }
}