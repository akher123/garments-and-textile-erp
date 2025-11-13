using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmBrandViewModel:OM_Brand
    {
        public List<OM_Brand> Brands { get; set; }

        public OmBrandViewModel()
        {
            Brands=new List<OM_Brand>();
        }
    }
}