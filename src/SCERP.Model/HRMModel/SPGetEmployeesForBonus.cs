using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public partial class SPGetEmployeesForBonus
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
        public string BonusDate { get; set; }
        public Nullable<int> ServiceLength { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BonusAmount { get; set; }
    }
}