using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class CuttFabRejectViewModel : ProSearchModel<CuttFabRejectViewModel>
    {
        public List<VwCuttFabReject> CuttFabRejects { get; set; }
        public PROD_CuttFabReject CuttFabReject { get; set; }
        public IEnumerable Items { get; set; }
        public CuttFabRejectViewModel()
        {
            Items = new List<object>();
            CuttFabRejects = new List<VwCuttFabReject>();
            CuttFabReject=new PROD_CuttFabReject();
        }
        public IEnumerable<SelectListItem> ItemSelectListItem
        {
            get { return new SelectList(Items, "BatchDetailId", "ItemName"); }
        }

       
    }
}