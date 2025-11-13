
namespace SCERP.Model.InventoryModel
{
   public class VwBookingDetailReport
    {
        public string BookingRefId { get; set; }
        public long BookingId { get; set; }
        public long BuyerId { get; set; }
        public int SupplierId { get; set; }
        public int MarchandiserId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string CompId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public System.DateTime BookingDate { get; set; }
        public int OrderQty { get; set; }
        public string PiNo { get; set; }
        public string BuyerName { get; set; }
        public string Merchandiser { get; set; }
        public string Supplier { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public decimal Rate { get; set; }
        public int ItemId { get; set; }
       public decimal ReceivedQty { get; set; }
       public int StoreId { get; set; }
       public string FColorRefId { get; set; }
       public string FColorName { get; set; }

    }
}
