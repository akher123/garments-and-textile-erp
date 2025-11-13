using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class HrmOTSummaryReport
    {
        public System.DateTime TransactionDate { get; set; }
        public Nullable<int> NoOfEmployee { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public Nullable<decimal> OTAmount { get; set; }
        public Nullable<decimal> ExtraOTHours { get; set; }
        public Nullable<decimal> ExtraOTAmount { get; set; }
        public Nullable<decimal> WeekendOTHours { get; set; }
        public Nullable<decimal> WeekendOTAmount { get; set; }
        public Nullable<decimal> HolidayOTHours { get; set; }
        public Nullable<decimal> HolidayOTAmount { get; set; }
    }
}
