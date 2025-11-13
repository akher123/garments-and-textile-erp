using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
   public class VwReDyeingFabricReceiveDetail
    {
        public long ReDyeingFabricReceiveDetailId { get; set; }
        public long ReDyeingFabricReceiveId { get; set; }
        public long BatchId { get; set; }
        public string BatchNo { get; set; }
        public string BtRefNo { get; set; }
        public string ItemName { get; set; }
        public long BatchDetailId { get; set; }
        public double RQty { get; set; }
    }
}
