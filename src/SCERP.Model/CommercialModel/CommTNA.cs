using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommTNA
    {
        public int CommTnaRowId { get; set; }
        public int LCRefId { get; set; }
        public string CompId { get; set; }
        public Nullable<int> SerialId { get; set; }
        public string Activity { get; set; }
        public string PlanDate { get; set; }
        public string ActualDate { get; set; }

        public string ActivityStatus { get; set; }
        public string Remarks { get; set; }
    }
}
