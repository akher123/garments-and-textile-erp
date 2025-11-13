using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class ItemModeViewModel:OM_ItemMode
    {
        public List<OM_ItemMode> ItemModes { get; set; }
        public ItemModeViewModel()
        {
            ItemModes=new List<OM_ItemMode>();
        }
    }
}