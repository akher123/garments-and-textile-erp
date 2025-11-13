using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.Web.Areas.Inventory.Models.ViewModels
{
    public class LotViewModel:OM_Color
    {
        public List<VwLot> OmColors { get; set; }
        public List<Inventory_Brand> Brands { get; set; }
        public LotViewModel()
        {
          OmColors=new List<VwLot>();
          Brands=new List<Inventory_Brand>();
        }
        [Required(ErrorMessage = "Required")]
        public string BrandId { get; set; }
        public List<Inventory_Brand> InventoryBrands { get; set; }
        public IEnumerable<SelectListItem> BrandSelectListItem
        {
            get
            {
                return new SelectList(Brands, "BrandId", "Name");
            }
        }
    }
}