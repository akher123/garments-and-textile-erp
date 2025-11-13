using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_BookingDetail
    {

        public long BookingDetailId { get; set; }
        public long BookingId { get; set; }
        public string CompId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public string FColorRefId { get; set; }
        public virtual Inventory_Booking Inventory_Booking { get; set; }
    }
}
