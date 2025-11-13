using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeMandatoryInfoViewModel : Employee
    {
        public EmployeeMandatoryInfoViewModel()
        {
            Employee = new Employee();
            EmployeeCompanyInfo = new EmployeeCompanyInfo();
            EmployeePresentAddress = new EmployeePresentAddress();
            IsSearch = true;
        }

        public Employee Employee { get; set; }

        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }
    }
}
