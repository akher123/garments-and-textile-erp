using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.HRMModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class HouseKeepingRegisterViewModel:ProSearchModel<HouseKeepingRegisterViewModel>
    {
        public List<HouseKeepingRegister> HouseKeepingRegisters { get; set; }
        public HouseKeepingRegister HouseKeepingRegister { get; set; }
        public List<HouseKeepingItem> HouseKeepingItems { get; set; }
       
        public HouseKeepingRegisterViewModel()
        {
            HouseKeepingRegisters = new List<HouseKeepingRegister>();
            HouseKeepingRegister = new HouseKeepingRegister();
        }
        public IEnumerable<SelectListItem> HkrSelectListItem
        {
            get
            {
                return new SelectList(HouseKeepingItems, "HouseKeepingItemId", "Name");
            }
        }
    }
}