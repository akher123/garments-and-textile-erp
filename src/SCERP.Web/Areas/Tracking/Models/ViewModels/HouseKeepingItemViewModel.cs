using SCERP.Model.HRMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.Tracking.Models.ViewModels
{
    public class HouseKeepingItemViewModel : HouseKeepingItem
    {

        public HouseKeepingItemViewModel()
        {
            HouseKeepingItems = new List<HouseKeepingItem>();
            HouseKeepingItem = new HouseKeepingItem();
            
        }

        public List<HouseKeepingItem> HouseKeepingItems { get; set; }
        public HouseKeepingItem HouseKeepingItem { get; set; }
    }
}