using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwFinishFabricIssueDetail
    {
        public long FinishFabricIssueDetailId { get; set; }
        public long FinishFabricIssueId { get; set; }
        public long BatchId { get; set; }
        public string BatchNo { get; set; }
        public string ItemName { get; set; }
        public long BatchDetailId { get; set; }
        public int NoOfRoll { get; set; }
        public double FabQty { get; set; }
        public double GreyWt { get; set; }
        public double IssueQty { get; set; }
        public double? CcuffQty { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
    }
}
