using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.InventoryModel
{
   public class Inventory_Booking
    {
        public Inventory_Booking()
        {
            this.Inventory_BookingDetail = new HashSet<Inventory_BookingDetail>();
        }

        public long BookingId { get; set; }
        public string CompId { get; set; }
        public string BookingRefId { get; set; }
        public string PiNo { get; set; }
          [Required(ErrorMessage = @"Required!")]
        public System.DateTime BookingDate { get; set; }
       [Required(ErrorMessage = @"Required!")]
        public int SupplierId { get; set; }
         [Required(ErrorMessage = @"Required!")]
        public string OrderNo { get; set; }
         [Required(ErrorMessage = @"Required!")]
        public string StyleNo { get; set; }
        [Required(ErrorMessage = @"Required!")]
        public int? OrderQty { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.CommonErrorMessage)]
        public int MarchandiserId { get; set; }
         [Required(ErrorMessage = @"Required!")]
        public long BuyerId { get; set; }
        public int StoreId { get; set; }
        public string Remarks { get; set; }
       public string OrderStyleRefId { get; set; }
      
        public virtual ICollection<Inventory_BookingDetail> Inventory_BookingDetail { get; set; }
        public virtual Mrc_SupplierCompany Mrc_SupplierCompany { get; set; }
        public virtual OM_Buyer OM_Buyer { get; set; }
        public virtual OM_Merchandiser OM_Merchandiser { get; set; }
    }
}
