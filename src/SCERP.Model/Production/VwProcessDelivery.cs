using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class VwProcessDelivery
    {
        public long ProcessDeliveryId { get; set; }
        public long PartyId { get; set; }
        public string RefNo { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ProcessRefId { get; set; }
        public string InvoiceNo { get; set; }
        public System.DateTime InvDate { get; set; }
        public string CompId { get; set; }
        public System.Guid PreparedBy { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public string Remarks { get; set; }
        public string OrderName { get; set; }
        public string BuyerName { get; set; }
        public string StyleName { get; set; }
        public string PartyName { get; set; }
        public int TotalQuantity { get; set; }
    }
}
