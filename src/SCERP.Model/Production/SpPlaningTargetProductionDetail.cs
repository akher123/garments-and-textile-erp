using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class SpPlaningTargetProductionDetail
    {
       public string OrderNo { get; set; }
       public string MachineRefId { get; set; }

       public string OrderStyleRefId { get; set; }
       public string StyleName { get; set; }
       public int StyleQty { get; set; }
       public string ProcessName { get; set; }
       public string Line { get; set; }
       public DateTime FromDate { get; set; }
       public DateTime ToDate { get; set; }
    }
}
