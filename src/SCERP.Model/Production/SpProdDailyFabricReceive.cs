using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class SpProdDailyFabricReceive
    {
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string BuyerRefId { get; set; }
        public string BuyerName { get; set; }
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string ItemName { get; set; }
        public string ComponentName { get; set; }
        public Nullable<decimal> GSM { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> ConsPDZ { get; set; }
        public Nullable<decimal> ReqFabric { get; set; }
        public string ConsRefId { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        public Nullable<decimal> TotalFabricQty { get; set; }
        public Nullable<decimal> ToDayReceive { get; set; }
        public Nullable<decimal> ToDayReceived { get; set; }
        public decimal? BalanceQty
        {
            get { return ReqFabric - TotalFabricQty; }
        }
    }
}
