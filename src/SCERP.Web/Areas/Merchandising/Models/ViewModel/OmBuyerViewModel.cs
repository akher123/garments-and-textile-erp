using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class OmBuyerViewModel:OM_Buyer
    {
        public IEnumerable Cities { get; set; }
        public List<Country> Countries { get; set; }
        public List<OM_Buyer> Buyers { get; set; }
    
        public OmBuyerViewModel()
        {
            Countries=new List<Country>();
            Cities = new List<object>();
           Buyers=new List<OM_Buyer>(); 
        }

     
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
        public List<SelectListItem> CitySelectListItem
        {
            get { return new SelectList(Cities, "CityId", "CityName").ToList(); }

        }
    }
}