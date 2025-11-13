using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class CommBbLcPurchase : ProSearchModel<CommBbLcPurchase>
    {
        public int BbLcPurchaseId { get; set; }
        public Nullable<int> BbLcRefId { get; set; }
        public string BbLcNo { get; set; }
        public Nullable<long> PurchaseOrderRefId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string CompId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
