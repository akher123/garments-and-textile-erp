using System;

namespace SCERP.Model.CommercialModel
{
   public class VwCommPurchaseOrder
    {
        public long PurchaseOrderId { get;  set; }
        public string CompId { get; set; }
        public string PurchaseOrderRefId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public Guid? UserId { get; set; }
        public string PType { get; set; }
        public string Rmks { get; set; }
        public bool IsApproved { get; set; }
        public Guid? IsApprovedBy{ get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string MerchandiserId { get; set; }
       public string BuyerRefId { get; set; }

       
    }
}
