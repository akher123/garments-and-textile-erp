using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class CompanyViewModel : Company
    {
        public List<Company> Companies { get; set; }

        public CompanyViewModel()
        {
            Companies = new List<Company>();
            IsSearch = true;
        }


        public string SearchByCompanyName
        {
            get;
            set;
        }

    }
}