using Salary;

namespace SCERP.BLL.Process.Salary
{
    public interface ISalaryProcess
    {
        ISalary CalculateProvedentFund();
        ISalary CalculateIncomeTax();
        ISalary CalculateOtherIncome();
    }
}
