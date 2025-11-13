using System;
using System.Collections.Generic;

namespace SCERP.Model.InventoryModel
{
   
    
    public partial class Inventory_MaterialReceivedDetail
    {

        public long MaterialReceivedDetailId { get; set; }
        public string CompId { get; set; }
        public long MaterialReceivedId { get; set; }
        public string Item { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string LotNo { get; set; }
        public string UnitName { get; set; }
        public string BuyerNameDtl { get; set; }
        public string OrderNameDtl { get; set; }
        public string StyleNameDtl { get; set; }
        public Nullable<double> ReceivedQty { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public virtual Inventory_MaterialReceived Inventory_MaterialReceived { get; set; }
    }
}
