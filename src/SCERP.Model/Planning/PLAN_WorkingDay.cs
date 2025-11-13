using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
    public class PLAN_WorkingDay
    {
        public long WorkingDayId { get; set; }
        public string CompId { get; set; }
        public System.DateTime WorkingDate { get; set; }
        public int DayStatus { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
