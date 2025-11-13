using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Maintenance
{
    
    
    public partial class VwReturnableChallanReceive
    {
        public string ReturnableChallanRefId { get; set; }
        public long ReturnableChallanReceiveId { get; set; }
        public string Messrs { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public string RefferancePerson { get; set; }
        public string CompId { get; set; }
        public long ReturnableChallanDetailId { get; set; }
        public long ReturnableChallanId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public double DeliveryQty { get; set; }
        public string BatchNo { get; set; }
        public int? RollQty { get; set; }
        public string Buyer { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string Color { get; set; }
        public Nullable<double> RemainingQty { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public double ReceiveQty { get; set; }
        public Nullable<double> RejectQty { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> TotalReceiveQty { get; set; }
        public Nullable<double> TotalRejectQty { get; set; }
    }
}
