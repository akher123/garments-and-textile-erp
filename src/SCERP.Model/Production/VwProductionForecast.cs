using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class VwProductionForecast
    {
        public long? TargetQty { get; set; }
        public long? ProductionQty { get; set; }
        public long? RemainingtQty { get; set; }
        public long? ProductionRate { get; set; }
        public long? ForecastQty { get; set; }
        public long? ShortQty { get; set; }
    }
}
