using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Common;

namespace SCERP.Model
{
    public class EmployeeMandatoryInfoViewModel : BaseModel
    {
        public EmployeeMandatoryInfoViewModel()
        {
            Employee = new Employee();
            EmployeeCompanyInfo = new EmployeeCompanyInfo();
            EmployeePresentAddress = new EmployeePresentAddress();
        }

        public Employee Employee { get; set; }
        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public EmployeePresentAddress EmployeePresentAddress { get; set; }

    }
}