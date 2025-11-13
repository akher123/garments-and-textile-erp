using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommGetOrderWithoutLcReport
    {
        public Nullable<int> LcRefId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderRefNo { get; set; }
        public string BuyerName { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public string Merchandiser { get; set; }
        public string Fabrication { get; set; }
        public string SeasonName { get; set; }
        public Nullable<decimal> StyleQuantity { get; set; }
    }
}
