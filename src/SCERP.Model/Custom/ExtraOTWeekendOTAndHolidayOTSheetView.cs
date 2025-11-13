using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class ExtraOTWeekendOTAndHolidayOTSheetView
    {
        public string EmployeeCategory { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UnitName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string CompanyAddress { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        public string Company { get; set; }
        public int? BranchId { get; set; }
        public string Branch { get; set; }
        public int? BranchUnitId { get; set; }
        public string Unit { get; set; }
        public int? BranchUnitDepartmentId { get; set; }
        public string Department { get; set; }
        public int? DepartmentSectionId { get; set; }
        public string Section { get; set; }
        public int? DepartmentLineId { get; set; }
        public string Line { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string JoiningDate { get; set; }
  	    public string QuitDate { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> TotalExtraOTHours { get; set; }
        public Nullable<decimal> TotalWeekendOTHours { get; set; }
        public Nullable<decimal> TotalHolidayOTHours { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> TotalExtraOTAmount { get; set; }
        public Nullable<decimal> TotalWeekendOTAmount { get; set; }
        public Nullable<decimal> TotalHolidayOTAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string MonthYear { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
