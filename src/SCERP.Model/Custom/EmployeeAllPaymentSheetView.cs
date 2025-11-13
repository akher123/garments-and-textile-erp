using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class EmployeeAllPaymentSheetView
    {
        public string EmployeeCategory { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string JoiningDate { get; set; }
        public string QuitDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public Nullable<int> BranchUnitId { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TotalOthersOT { get; set; }
        public Nullable<decimal> AmountToBePaid { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> EmployeeTypeId { get; set; }
        public string MonthName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
