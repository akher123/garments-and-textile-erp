using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.InventoryModel
{
  
    
    public partial class Inventory_StyleShipmentDetail
    {
        public long StyleShipmentDetailId { get; set; }
        public string CompId { get; set; }
        public long StyleShipmentId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        [Required]
        public int ShipmentQty { get; set; }
        public virtual Inventory_StyleShipment Inventory_StyleShipment { get; set; }
    }
}
