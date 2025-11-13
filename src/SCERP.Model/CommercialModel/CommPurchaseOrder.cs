using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.CommercialModel
{
    public class CommPurchaseOrder : ProSearchModel<CommPurchaseOrder>
    {
      public CommPurchaseOrder()
        {
            this.CommPurchaseOrderDetail = new HashSet<CommPurchaseOrderDetail>();
        }
        public long PurchaseOrderId { get;  set; }
        public string CompId { get; set; }
        public string PurchaseOrderRefId { get; set; }
        [Required]
        public string PurchaseOrderNo { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public int SupplierId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public Guid? UserId { get; set; }
        public string PType { get; set; }
        public string Rmks { get; set; }
        public bool IsApproved { get; set; }
        public Guid? ApprovedBy { get; set; }
        public virtual ICollection<CommPurchaseOrderDetail> CommPurchaseOrderDetail { get; set; }
    }
}
