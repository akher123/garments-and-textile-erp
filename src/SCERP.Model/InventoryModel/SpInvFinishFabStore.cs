using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class SpInvFinishFabStore
    {
        public long DyeingSpChallanDetailId { get; set; }
        public string SubProcessName { get; set; }
        public string PartyName { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string StyleName { get; set; }

        public string BatchNo { get; set; }
        public string BtRefNo { get; set; }
        public long BatchId { get; set; }
        public long BatchDetailId { get; set; }
        public string FabType { get; set; }
        public double FabQty { get; set; }
        public double RcvQty { get; set; }
        public double GreyWt { get; set; }
        public double CcuffQty { get; set; }
        public string Remarks { get; set; }
        public double BalanceQty { get { return FabQty - RcvQty; } }
        public double Qty { get; set; }
    }
}
