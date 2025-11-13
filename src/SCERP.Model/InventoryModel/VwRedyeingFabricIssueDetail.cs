using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwRedyeingFabricIssueDetail
    {
        public long RedyeingFabricIssueDetailId { get; set; }
        public long RedyeingFabricIssueId { get; set; }
        public long BatchId { get; set; }
        public long BatchDetailId { get; set; }
        public double ReprocessQty { get; set; }
        public double FinishQty { get; set; }
        public int NoRole { get; set; }
        public string Remarks { get; set; }
        public string BatchNo { get; set; }
        public string ItemName { get; set; }
    }
}
