using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.Process.Salary;

namespace Salary
{
    public class IncomeTax : ISalary
    {
        public double CalculateSalarySigment()
        {
            double salary = 130.25;
            return salary;
        }
    }
}
