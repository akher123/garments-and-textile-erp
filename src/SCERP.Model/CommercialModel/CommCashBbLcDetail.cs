using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class CommCashBbLcDetail
    {
        public int CashBbLcDetailsId { get; set; }
        public int BbLcId { get; set; }
        public string Item { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Rate { get; set; }
        public string Remarks { get; set; }

        public virtual CommCashBbLcInfo CommCashBbLcInfo { get; set; }
    }
}
