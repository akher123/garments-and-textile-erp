using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class SpRejectYarnDetail
    {
        public long MaterialReceiveDetailId { get; set; }
        public string RefNo { get; set; }
        public string MRRNO { get; set; }
        public System.DateTime MRRDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiveRegNo { get; set; }
        public string GateEntryNo { get; set; }
        public string PoNo { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string Brand { get; set; }
        public string YarnCount { get; set; }
        public string YarnLot { get; set; }
        public string SupplierName { get; set; }
        public decimal ReceivedQty { get; set; }
        public double RejectedQty { get; set; }
        public decimal ReceivedRate { get; set; }
        [Required]
        public double Qty { get; set; }
    }
}
