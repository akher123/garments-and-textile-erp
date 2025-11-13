using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.Model.Production
{
    public class PROD_BatchDetail
    {
        public PROD_BatchDetail()
        {
            this.PROD_DyeingSpChallanDetail = new HashSet<PROD_DyeingSpChallanDetail>();
            this.Inventory_FinishFabDetailStore = new HashSet<Inventory_FinishFabDetailStore>();
        }
        public long BatchDetailId { get; set; }
        public string CompId { get; set; }
        public long BatchId { get; set; }
        public int ItemId { get; set; }
        public string FdiaSizeRefId { get; set; }
        public string MdiaSizeRefId { get; set; }
        public string ComponentRefId { get; set; }
        public string GSM { get; set; }
        public double Quantity { get; set; }
        public string Remarks { get; set; }
        public double? Rate { get; set; }
        public double? StLength { get; set; }
        public double? FLength { get; set; }
        public double? RollQty { get; set; }

        public virtual Pro_Batch Pro_Batch { get; set; }
        public virtual ICollection<Inventory_FinishFabDetailStore> Inventory_FinishFabDetailStore { get; set; }
        public virtual ICollection<PROD_DyeingSpChallanDetail> PROD_DyeingSpChallanDetail { get; set; }
    }
}
