using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class PoliceStationViewModel:PoliceStation
    {     
        public PoliceStationViewModel()
        {
            PoliceStations = new List<PoliceStation>();
            IsSearch = true;
        }

        public List<PoliceStation> PoliceStations { get; set; }

        public int SearchByCountry
        {
            get;
            set;
        }

        public string SearchByPoliceStation
        {
            get;
            set;
        }

        public int SearchByDistrict
        {
            get;
            set;
        }
    }
}