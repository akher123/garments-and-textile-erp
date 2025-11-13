using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class CountryViewModel : Country
    {
        public CountryViewModel()
        {
            Countries = new List<Country>();
            IsSearch = true;
        }

        public List<Country> Countries { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}