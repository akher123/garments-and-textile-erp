using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmCategoryViewModel:OM_Category
    {
        public List<OM_Category> Categories { get; set; }
        public OmCategoryViewModel()
        {
            Categories=new List<OM_Category>();
        }
    }
}