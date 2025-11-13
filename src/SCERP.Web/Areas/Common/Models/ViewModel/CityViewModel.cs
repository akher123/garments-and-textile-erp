using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class CityViewModel:City
    {
        public List<State> States { get; set; }
        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
        public CityViewModel()
        {
            this.Cities=new List<City>();
            this.States=new List<State>();
            this.Countries=new List<Country>();
        }
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(Countries, "Id", "CountryName").ToList(); }

        }
        public List<SelectListItem> StateSelectListItem
        {
            get { return new SelectList(States, "StateId", "StateName").ToList(); }

        }
    }
}