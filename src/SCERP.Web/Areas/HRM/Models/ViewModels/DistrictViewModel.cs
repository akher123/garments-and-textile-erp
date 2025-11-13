using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class DistrictViewModel : District
    {
        public DistrictViewModel()
        {
            Districts = new List<District>();
            IsSearch = true;
        }

        public List<District> Districts { get; set; }

        public List<Country> Countries { get; set; }
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }


        public int SearchByCountry
        {
            get;
            set;
        }

        public string SearchByDistrict
        {
            get;
            set;
        }

    }
}