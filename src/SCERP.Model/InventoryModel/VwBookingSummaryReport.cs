using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwBookingSummaryReport
    {
        public string BookingRefId { get; set; }
        public long BookingId { get; set; }
        public long BuyerId { get; set; }
        public int SupplierId { get; set; }
        public int MarchandiserId { get; set; }
        public string CompId { get; set; }
        public string OrderNo { get; set; }
        public string StyleNo { get; set; }
        public System.DateTime BookingDate { get; set; }
        public int OrderQty { get; set; }
        public string PiNo { get; set; }
        public string BuyerName { get; set; }
        public string Merchandiser { get; set; }
        public string Supplier { get; set; }
        public decimal Amount { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
       public int StoreId { get; set; }
      
    }
}
