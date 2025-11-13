
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.InventoryModel
{
    public class VwBookingDetail
    {
        public long BookingDetailId { get; set; }
        public long BookingId { get; set; }
        public string CompId { get; set; }
 
        public string ColorRefId { get; set; }

        public string ColorName { get; set; }

        public string SizeName { get; set; }
       
        public string SizeRefId { get; set; }
        [Required(ErrorMessage = @"Required!")]
        public int ItemId { get; set; }
         [Required(ErrorMessage = @"Required!")]
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string FColorRefId { get; set; }
        public string FColorName { get; set; }
    }
}
