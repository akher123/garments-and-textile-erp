using SCERP.Model;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class EmployeeViewModel : Employee
    {
        public Employee Employee { get; set; }

        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }

    }
}
