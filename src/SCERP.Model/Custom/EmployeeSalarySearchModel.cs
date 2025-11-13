using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class EmployeeSalarySearchModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string EmployeeType { get; set; }
        public string CompanyName { get; set; }
        public string Branch { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string JoiningDate { get; set; }


        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Nullable<decimal> LWP { get; set; }
        public Nullable<decimal> Absent { get; set; }
        public Nullable<decimal> Advance { get; set; }
        public Nullable<decimal> Stamp { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> AttendanceBonus { get; set; }
        public Nullable<decimal> ShiftingAllowance { get; set; }
        public Nullable<decimal> PayableAmount { get; set; }
        public Nullable<double> OTHours { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> OTAmount { get; set; } 
       

        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public Nullable<decimal> MobileBill { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> ProvidentFund { get; set; }
        public Nullable<decimal> NetSalaryPaid { get; set; }
        public string Comments { get; set; }


        public int DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public int? LineId { get; set; }
        public bool IsActive { get; set; }
    }
}
