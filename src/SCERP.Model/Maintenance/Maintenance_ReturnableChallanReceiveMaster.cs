using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Maintenance
{
   
    
    public partial class Maintenance_ReturnableChallanReceiveMaster
    {
        public Maintenance_ReturnableChallanReceiveMaster()
        {
            this.Maintenance_ReturnableChallanReceive = new HashSet<Maintenance_ReturnableChallanReceive>();
        }
    
        public long ReturnableChallanReceiveMasterId { get; set; }
        public string RetChallanMasterRefId { get; set; }
        public Nullable<long> ReturnableChallanId { get; set; }
        [Required]
        public string ChallanNo { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public double TotalAmount { get; set; }
        public string CompId { get; set; }
    
        public virtual Maintenance_ReturnableChallan Maintenance_ReturnableChallan { get; set; }
        public virtual ICollection<Maintenance_ReturnableChallanReceive> Maintenance_ReturnableChallanReceive { get; set; }
    }
}
