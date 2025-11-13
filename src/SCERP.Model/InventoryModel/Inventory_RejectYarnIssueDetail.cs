
namespace SCERP.Model.InventoryModel
{
    public class Inventory_RejectYarnIssueDetail
    {
        public int RejectYarnIssueDetailId { get; set; }
        public int RejectYarnIssueId { get; set; }
        public long MaterialReceiveDetailId { get; set; }
        public double Qty { get; set; }

        public virtual Inventory_RejectYarnIssue Inventory_RejectYarnIssue { get; set; }

    }
}
