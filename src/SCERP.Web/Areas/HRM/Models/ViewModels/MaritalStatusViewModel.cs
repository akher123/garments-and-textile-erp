using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class MaritalStatusViewModel : MaritalState
    {

        public MaritalStatusViewModel()
        {
            MaritalStatuses = new List<MaritalState>();
            IsSearch = true;
        }

        public List<MaritalState> MaritalStatuses { get; set; }


        public string SearchKey
        {
            get;
            set;
        }

    }
}