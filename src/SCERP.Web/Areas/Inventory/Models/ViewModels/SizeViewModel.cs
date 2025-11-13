using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class SizeViewModel:Inventory_Size
    {
        public List<Inventory_Size> InventorySizes { get; set; }
        public string SizeName { get; set; }
        public SizeViewModel()
        {
            this.InventorySizes=new List<Inventory_Size>();
        }
    }
}