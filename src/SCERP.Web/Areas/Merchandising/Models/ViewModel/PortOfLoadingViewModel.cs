using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class PortOfLoadingViewModel:OM_PortOfLoading
    {
        public List<OM_PortOfLoading> PortOfLoadings { get; set; }
        public List<Country> Countries { get; set; }
        public PortOfLoadingViewModel()
        {
            Countries=new List<Country>();
            PortOfLoadings=new List<OM_PortOfLoading>();
        }
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
    }
}