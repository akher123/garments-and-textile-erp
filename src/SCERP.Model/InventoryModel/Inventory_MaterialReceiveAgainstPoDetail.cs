using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public partial  class Inventory_MaterialReceiveAgainstPoDetail
    {
        public long MaterialReceiveAgstPoDetailId { get; set; }
        public long MaterialReceiveAgstPoId { get; set; }
        public string CompId { get; set; }
        public int ItemId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal ReceivedRate { get; set; }
        public Nullable<decimal> RejectedQty { get; set; }
        public Nullable<decimal> DiscountQty { get; set; }
        public string LotNo { get; set; }
        public string FColorRefId { get; set; }
        public string GSizeRefId { get; set; }
        public string Location { get; set; }
        public long? PurchaseOrderDetailId { get; set; }
        public string OrderStyleRefId { get; set; }
        public virtual Inventory_MaterialReceiveAgainstPo Inventory_MaterialReceiveAgainstPo { get; set; }
    }
}
