using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
   public class VwCompConsumptionOrderStyle
    {
        public string MerchandiserId { get; set; }
        public string CompId { get; set; }
        public string StyleName { get; set; }
        public string SeasonName { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string RefNo { get; set; }
        public string BuyerRef { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<int> TotalComponet { get; set; }
        public Nullable<decimal> TotalFabQty { get; set; }
    
    }
}
