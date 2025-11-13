using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.InventoryModel
{
    
    
    public partial class Inventory_MaterialReceived
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_MaterialReceived()
        {
            this.Inventory_MaterialReceivedDetail = new HashSet<Inventory_MaterialReceivedDetail>();
        }
    
        public long MaterialReceivedId { get; set; }
        public string MaterialReceivedRefId { get; set; }
        [Required]
        public string CompId { get; set; }
        [Required]
        public string GRN { get; set; }
        [Required]
        public string GEN { get; set; }
        [Required]
        public System.DateTime ReceivedDate { get; set; }
        [Required]
        public string ChallanNo { get; set; }
        [Required]
        public Nullable<System.DateTime> ChallanDate { get; set; }
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string BuyerName { get; set; }
        [Required]
        public string OrderNo { get; set; }
        [Required]
        public string StyleNo { get; set; }
        public string Article { get; set; }
        [Required]
        public string LCNo { get; set; }
        [Required]
        public string BillStatus { get; set; }
        public string Remarks { get; set; }
        public string RegisterType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_MaterialReceivedDetail> Inventory_MaterialReceivedDetail { get; set; }
    }
}
