using System;

namespace SCERP.Model.CommercialModel
{
    public class VwPurchaseOrderDetail
    {
        public long PurchaseOrderDetailId { get; set; }
        public long PurchaseOrderId { get; set; }
        public string CompId { get; set; }
        public string PurchaseOrderRefId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string GColorRefId { get; set; }
        public string GSizeRefId { get; set; }
        public string GColorName { get; set; }
        public string GSizeName { get; set; }

        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> xRate { get; set; }
        public string UnitName { get; set; }
    }
}
