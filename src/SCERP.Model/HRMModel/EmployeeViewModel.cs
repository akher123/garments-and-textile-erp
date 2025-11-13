using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }

        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }

        public EmployeePermanentAddress EmployeePermanentAddress { get; set; }
 

    }
}
