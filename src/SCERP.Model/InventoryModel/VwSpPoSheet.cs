using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class VwSpPoSheet
    {

        public long PurchaseOrderId { get; set; }
        public long PurchaseOrderDetailId { get; set; }
        public string CompId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string GColorName { get; set; }
        public string GColorRefId { get; set; }
        public string GSizeRefId { get; set; }
        public string GSizeName { get; set; }
        public string UnitName { get; set; }
        public string StyleName { get; set; }
        public string PurchaseOrderRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> TotalRcvQty { get; set; }

        public Nullable<decimal> xRate { get; set; }
    }
}
