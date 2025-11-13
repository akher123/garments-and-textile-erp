using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class VwBbLcPurchaseCommon
    {
        public int BbLcPurchaseId { get; set; }
        public Nullable<int> BbLcRefId { get; set; }
        public string BbLcNo { get; set; }
        public Nullable<long> PurchaseOrderRefId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Nullable<DateTime> PurchaseDate { get; set; }
        public string ItemCode { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> xRate { get; set; }
        public string PurchaseType { get; set; }
        public string CompId { get; set; }
        public string BbLcNo1 { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
    }
}
