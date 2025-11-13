using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class EmployeeCardPrintModel
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
        public string EmployeeJobType { get; set; }
        public string JoiningDate { get; set; }
        public string IssueDate { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public int? LineId { get; set; }
        public string PhotographPath { get; set; }
        public bool IsActive { get; set; }

        public string CardValidity { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string MobileNo { get; set; }
        public string BloodGroup { get; set; }
        public string FatherName { get; set; }
        public string Village { get; set; }
        public string PostOffice { get; set; }
        public string PoliceStation { get; set; }
        public string District { get; set; }
        public string EmergencyContactNo { get; set; }
        public string NationalIdNo { get; set; }
        public string BirthCertificateNo { get; set; }
        public string Notice1 { get; set; }
        public string Notice2 { get; set; }
        public bool IsBangla { get; set; }
    }
}
