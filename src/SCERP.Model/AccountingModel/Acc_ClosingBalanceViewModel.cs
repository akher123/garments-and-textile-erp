using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
   public class Acc_ClosingBalanceViewModel
   {
       public int SectorId { get; set; }
       public int CostCentre { get; set; }
       public int FpId { get; set; }
       public int GlId { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public string VoucherType { get; set; }
       public string VoucherTypeName { get; set; }
       public string Amount { get; set; }
    }
}
