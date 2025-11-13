using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class VwCommBbLcPurchase
    {
        public int BbLcPurchaseId { get; set; }
        public string BbLcNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string ExpDate { get; set; }
        public string ProductType { get; set; }
        public string OrderNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string SupplierName { get; set; }
        public Nullable<decimal> TotalQuantity { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    }
}
