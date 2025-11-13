using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class InventoryApprovalStatusViewModel:Inventory_ApprovalStatus
    {
        public List<Inventory_ApprovalStatus> InventoryApprovalStatuses { get; set; }
        public string ApprovalStatusName { get; set; }
        public InventoryApprovalStatusViewModel()
        {
            InventoryApprovalStatuses=new List<Inventory_ApprovalStatus>();
        }
    }
}