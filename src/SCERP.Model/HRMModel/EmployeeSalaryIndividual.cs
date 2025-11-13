using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class EmployeeSalaryIndividual
    {
        public string EmployeeCardId { get; set; }
        public int Year { get; set; }
        public decimal NetSalary { get; set; }
        public decimal OTAmount { get; set; }
        public decimal ExtraOTAmount { get; set; }
        public decimal WeekendOTAmount { get; set; }
        public decimal HolidayOTAmount { get; set; }
        public decimal Bonus { get; set; }
    }
}
