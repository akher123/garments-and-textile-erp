using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MisModel
{
    public class MIS_UserActivity
    {
        public int UserActivityId { get; set; }
        public string ModuleName { get; set; }
        public string ActivityName { get; set; }
        public int ToDayData { get; set; }
        public int MonthlyData { get; set; }
        public int YearlyData { get; set; }
        public int SlNo { get; set; }
        public string CompId { get; set; }
    }
}
