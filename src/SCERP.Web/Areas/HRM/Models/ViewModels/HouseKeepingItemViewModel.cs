using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class HouseKeepingItemViewModel : ProSearchModel<HouseKeepingItemViewModel>
    {
        public List<HouseKeepingItem> HouseKeepingItems { get; set; }
        public HouseKeepingItem HouseKeepingItem { get; set; }
        public HouseKeepingItemViewModel()
        {
            HouseKeepingItem=new HouseKeepingItem();
            HouseKeepingItems=new List<HouseKeepingItem>();
        }
    }
}