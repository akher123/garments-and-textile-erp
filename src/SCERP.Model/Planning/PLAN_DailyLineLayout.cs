using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
   public  class PLAN_DailyLineLayout
    {
        public long LineLayoutId { get; set; }
        public string CompId { get; set; }
        public int LineId { get; set; }
        public System.DateTime OutputDate { get; set; }
        public int NumberOfMachine { get; set; }
        public int PlanQty { get; set; }
        public string Remarks { get; set; }

        public virtual Production_Machine Production_Machine { get; set; }
    }
}
