using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommBbLcPurchaseCommon : ProSearchModel<CommBbLcPurchaseCommon>
    {
        public int BbLcPurchaseId { get; set; }
        public Nullable<int> BbLcRefId { get; set; }
        public string BbLcNo { get; set; }
        public Nullable<long> PurchaseOrderRefId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public string ItemCode { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> xRate { get; set; }
        public string PurchaseType { get; set; }
        public string PType { get; set; }
        public string CompId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
