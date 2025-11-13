using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
   public class EmployeeBonusView
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string JoiningDate { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Line { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }                    
        public int DepartmentId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int EmployeeBonusId { get; set; }
        public decimal BonusAmount { get; set; }
        public string BonusDate { get; set; }
    }
}
