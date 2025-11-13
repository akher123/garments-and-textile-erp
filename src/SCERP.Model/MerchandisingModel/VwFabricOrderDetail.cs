using System;

namespace SCERP.Model.MerchandisingModel
{
    public class VwFabricOrderDetail
    {
        public int FabricOrderId { get; set; }
        public string FabricOrderRefId { get; set; }
        public string ActiveStatus { get; set; }
        public string OrderStyleRefId { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string CompId { get; set; }
        public string Merchandiser { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string ItemName { get; set; }
        public string BuyerRefId { get; set; }
        public decimal FinishQty { get; set; }
        public decimal GreyQty { get; set; }
        public decimal BookingQty { get; set; }

        public bool YLocked { get; set; }



    }
}
