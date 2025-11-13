using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class SPCommCMInfo
    {
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<int> LineId { get; set; }
        public string LineName { get; set; }
        public Nullable<int> NoOfEmployee { get; set; }
        public Nullable<decimal> TotalWorkingHours { get; set; }
        public Nullable<decimal> AverageWorkingHours { get; set; }
    }
}
