using System.Collections.Generic;
using SCERP.Model;
namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class PurchaseTypeViewModel:Inventory_PurchaseType
    {
        public List<Inventory_PurchaseType> InventoryPurchaseTypes { get; set; }
        public string PurchaseTypeTitle { get; set; }
        public PurchaseTypeViewModel()
        {
            this.InventoryPurchaseTypes=new List<Inventory_PurchaseType>();
        }
    }
}