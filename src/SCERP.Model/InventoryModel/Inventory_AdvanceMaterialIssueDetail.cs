using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_AdvanceMaterialIssueDetail
    {
        public long AdvanceMaterialIssueDetailId { get; set; }
        public long AdvanceMaterialIssueId { get; set; }
        public string CompId { get; set; }
        public int ItemId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public decimal IssueQty { get; set; }
        public decimal IssueRate { get; set; }
        public string FColorRefId { get; set; }
        public string GSizeRefId { get; set; }
        public string GColorRefId { get; set; }
        public decimal? QtyInBag { get; set; }
        public string Wrapper { get; set; }
        public long? PurchaseOrderDetailId { get; set; }
        public virtual Inventory_AdvanceMaterialIssue Inventory_AdvanceMaterialIssue { get; set; }
    }
}
