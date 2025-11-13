using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommLcOrderSummary
    {
        public string BuyerName { get; set; }
        public string LcNo { get; set; }
        public Nullable<System.DateTime> LcDate { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<decimal> LcQuantity { get; set; }
        public string BbLcNo { get; set; }
        public decimal BbLcAmount { get; set; }
        public int ExportQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal ExportAmount { get; set; }
        public Nullable<decimal> GrossProfit { get; set; }
    }
}
