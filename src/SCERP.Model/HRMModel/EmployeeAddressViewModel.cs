using SCERP.Model;

namespace SCERP.Model
{
    public class EmployeeAddressViewModel
    {
        public EmployeeAddressViewModel()
        {
            EmployeePresentAddress = new EmployeePresentAddress();
            EmployeePermanentAddress = new EmployeePermanentAddress();
        }

        public EmployeePresentAddress EmployeePresentAddress { get; set; }

        public EmployeePermanentAddress EmployeePermanentAddress { get; set; }
    }
}
