using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
   public class CommPurchaseOrderDetail
    {
        public long PurchaseOrderDetailId { get; set; }
        public long PurchaseOrderId { get; set; }
        public string CompId { get; set; }
        public string PurchaseOrderRefId { get; set; }
        public string ItemCode { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string GColorRefId { get; set; }
        public string GSizeRefId { get; set; }

        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> xRate { get; set; }

        public virtual CommPurchaseOrder CommPurchaseOrder { get; set; }
    }
}
