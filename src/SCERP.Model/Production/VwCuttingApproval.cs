using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwCuttingApproval
    {
        public string CompId { get; set; }
        public string ApprovalStatus { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        public long CuttingBatchId { get; set; }
        public string OrderRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string BuyerRefId { get; set; } 
        public Nullable<System.DateTime> CuttingDate { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderNo { get; set; }
        
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string ComponentName { get; set; }
        public string JobNo { get; set; }
        public int MarkerPcs { get; set; }
        public int Ply { get; set; }
        public int TotalQty { get; set; }
        public Nullable<int> RejectQty { get; set; }

       
    }
}
