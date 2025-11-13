using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class SalarySummaryView
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchName { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeCategory { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<int> TotalEmployee { get; set; }
        public Nullable<decimal> ExtraOTWeekendOTandHolidayOT { get; set; }
        public Nullable<decimal> TotalOTHours { get; set; }
        public Nullable<decimal> TotalOTAmount { get; set; }
        public Nullable<decimal> TotalExtraOTHours { get; set; }
        public Nullable<decimal> TotalExtraOTAmount { get; set; }
        public Nullable<decimal> TotalWeekendOTHours { get; set; }
        public Nullable<decimal> TotalWeekendOTAmount { get; set; }
        public Nullable<decimal> TotalHolidayOTHours { get; set; }
        public Nullable<decimal> TotalHolidayOTAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    }
}
