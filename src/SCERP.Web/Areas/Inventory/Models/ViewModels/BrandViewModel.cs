using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class BrandViewModel:Inventory_Brand
    {
        public List<Inventory_Brand> InventoryBrands { get; set; }
        public string BrandName { get; set; }
        public BrandViewModel()
        {
            InventoryBrands=new List<Inventory_Brand>();
        }
    }
}