using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class StateViewModel:State
    {
        public List<Country> CountryList { get; set; }
        public List<State> States{ get; set; }
        public StateViewModel()
        {
            CountryList=new List<Country>();
            States=new List<State>();
        }

      
        public List<SelectListItem> CountrySelectListItem
        {
            get { return new SelectList(CountryList, "Id", "CountryName").ToList(); }

        }
    }
}