using System;
using System.Collections.Generic;

namespace SCERP.Model.Maintenance
{
    
    
    public partial class Maintenance_ReturnableChallanDetail
    {
        public Maintenance_ReturnableChallanDetail()
        {
            this.Maintenance_ReturnableChallanReceive = new HashSet<Maintenance_ReturnableChallanReceive>();
        }
    
        public long ReturnableChallanDetailId { get; set; }
        public long ReturnableChallanId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public double DeliveryQty { get; set; }
        public double ReceiveQty { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public string BatchNo { get; set; }
        public int? RollQty { get; set; }
        public string Buyer { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public string Color { get; set; }

        public Nullable<double> RejectQty { get; set; }
        public virtual Maintenance_ReturnableChallan Maintenance_ReturnableChallan { get; set; }
        public virtual ICollection<Maintenance_ReturnableChallanReceive> Maintenance_ReturnableChallanReceive { get; set; }
    }
}
