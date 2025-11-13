using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class SPGetMaternityReport
    {
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public string NameInBengali { get; set; }
        public string MobileNo { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public string MonthName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameInBengali { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddressInBengali { get; set; }
        public string Branch { get; set; }
        public string BranchInBengali { get; set; }
        public string Unit { get; set; }
        public string UnitInBengali { get; set; }
        public string Department { get; set; }
        public string DepartmentInBengali { get; set; }
        public string Section { get; set; }
        public string SectionInBengali { get; set; }
        public string Line { get; set; }
        public string LineInBengali { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeTypeInBengali { get; set; }
        public string Grade { get; set; }
        public string GradeInBengali { get; set; }
        public string Designation { get; set; }
        public string DesignationInBengali { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> QuitDate { get; set; }
        public Nullable<int> WorkingDays { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<System.DateTime> LeaveEndDate { get; set; }
        public Nullable<decimal> Bonus { get; set; }
        public Nullable<System.DateTime> FirstPaymentDate { get; set; }
        public Nullable<decimal> FirstPaymentAmount { get; set; }
        public Nullable<System.DateTime> SecondPaymentDate { get; set; }
        public Nullable<decimal> SecondPaymentAmount { get; set; }
    }
}
