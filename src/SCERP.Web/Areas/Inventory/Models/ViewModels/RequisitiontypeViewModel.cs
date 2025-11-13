using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class RequisitiontypeViewModel:Inventory_RequsitionType
    {
        public List<Inventory_RequsitionType>InventoryRequsitionTypes { get; set; }
        public string RequsitionTypeTitle { get; set; }
        public RequisitiontypeViewModel()
        {
            InventoryRequsitionTypes=new List<Inventory_RequsitionType>();
        }
    }
}