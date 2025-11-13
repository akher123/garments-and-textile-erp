using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.InventoryModel
{
  
    
    public partial class Inventory_StyleShipment
    {
        public Inventory_StyleShipment()
        {
            this.Inventory_StyleShipmentDetail = new HashSet<Inventory_StyleShipmentDetail>();
        }
    
        public long StyleShipmentId { get; set; }
        public string StyleShipmentRefId { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        [Required]
        public string Messrs { get; set; }
        [Required]
        public string Address { get; set; }
        public string DepoName { get; set; }
        [Required]
        public string Through { get; set; }
        public string ThroughCellNo { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverLicenceNo { get; set; }
        public string DriverCellNo { get; set; }
        public string DriverNid { get; set; }
         [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string BuyerRefId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        [Required]
        public string OrderStyleRefId { get; set; }
        [Required]
        public string ShipmentMode { get; set; }
        public string CompId { get; set; }
        public Nullable<System.Guid> PrepairedBy { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<System.Guid> ApprovedBy { get; set; }
        public string LockNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? ShipDate { get; set; }
        public virtual ICollection<Inventory_StyleShipmentDetail> Inventory_StyleShipmentDetail { get; set; }
    }
}
