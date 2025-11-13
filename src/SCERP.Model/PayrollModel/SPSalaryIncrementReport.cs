using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.PayrollModel
{
    public class SPSalaryIncrementReport
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public string MobilePhone { get; set; }
        public string Percent { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HouseRent { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> EntertainmentAllowance { get; set; }
        public Nullable<decimal> FoodAllowance { get; set; }
        public Nullable<decimal> Conveyance { get; set; }
        public Nullable<System.DateTime> LastIncrementDate { get; set; }
        public Nullable<double> NewGross { get; set; }
        public Nullable<double> NewBasic { get; set; }
        public Nullable<double> NewHouseRent { get; set; }
        public Nullable<decimal> OtherBenefit { get; set; }
        public Nullable<decimal> OtherIncrement { get; set; }
        public Nullable<double> TotalIncrement { get; set; }
        public string ApprovedIncrement { get; set; }
        public string Remarks { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
