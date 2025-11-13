using System;
using System.Collections.Generic;

namespace SCERP.Model.Maintenance
{
    
    
    public partial class Maintenance_ReturnableChallanReceive
    {
        public long ReturnableChallanReceiveId { get; set; }
        public long ReturnableChallanDetailId { get; set; }
        public Nullable<long> ReturnableChallanReceiveMasterId { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public double ReceiveQty { get; set; }
        public string CompId { get; set; }
        public Nullable<double> RejectQty { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<double> Amount { get; set; }
        public virtual Maintenance_ReturnableChallanDetail Maintenance_ReturnableChallanDetail { get; set; }
        public virtual Maintenance_ReturnableChallanReceiveMaster Maintenance_ReturnableChallanReceiveMaster { get; set; }
    }
}
