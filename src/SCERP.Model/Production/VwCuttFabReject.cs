using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwCuttFabReject
    {
        public int CuttFabRejectId { get; set; }
        public string CompId { get; set; }
        public long BatchId { get; set; }
        public long BatchDetailId { get; set; }
        public double RejectWit { get; set; }
        public double CuttingWit { get; set; }
        public string ChallanNo { get; set; }
        public string Remarks { get; set; }
        public DateTime EntryDate { get; set; }
        public string BtRefNo { get; set; }
        public string BatchNo { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string ItemName { get; set; }
        public string GSM { get; set; }
        public string FdiaSizeName { get; set; }
        public string MdiaSizeName { get; set; }
    }
}
