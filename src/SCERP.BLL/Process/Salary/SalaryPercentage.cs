using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.Process.Salary
{
    public class SalaryPercentage
    {
        private readonly decimal _grossSalary;

        private readonly EmployeeGradeSalaryPercentage _employeeGradeSalaryPercentage;
        public SalaryPercentage(decimal grossSalary, EmployeeGradeSalaryPercentage employeeGradeSalaryPercentage)
           
        {
            this._grossSalary = grossSalary;
            this._employeeGradeSalaryPercentage = employeeGradeSalaryPercentage;
        }

        public decimal GetTotalAllowance()
        {
            return _employeeGradeSalaryPercentage.Medical + _employeeGradeSalaryPercentage.Conveyance +
                   _employeeGradeSalaryPercentage.Food;
        }

        public decimal BasicSalary
        {
            get { return (_grossSalary - GetTotalAllowance() )/ _employeeGradeSalaryPercentage.BasicPercentageRate; }
        }
        public decimal HouseRent
        {
            get { return (BasicSalary * _employeeGradeSalaryPercentage.HouseRentPercentage)/100; }
        }
        public decimal GetTotalSalary()
        {
            return GetTotalAllowance() + HouseRent + BasicSalary;
        }
    }
}
