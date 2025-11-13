using System;
using System.Collections.Generic;
    

namespace SCERP.Model.Maintenance
{
    
    public partial class VwChallanReceiveMaster
    {
        public long ReturnableChallanReceiveMasterId { get; set; }
        public string RetChallanMasterRefId { get; set; }
        public string ChallanNo { get; set; }
        public string CompId { get; set; }
        public long ReturnableChallanReceiveId { get; set; }
        public long ReturnableChallanDetailId { get; set; }
        public Nullable<double> RejectQty { get; set; }
        public double ReceiveQty { get; set; }
        public Nullable<double> Amount { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string Buyer { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string Color { get; set; }
        public string BatchNo { get; set; }
        public int? RollQty { get; set; }
        public double DeliveryQty { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> TotalReceiveQty { get; set; }
        public Nullable<double> TotalRejectQty { get; set; }
        public Nullable<double> RemainingQty { get; set; }
    }
}
