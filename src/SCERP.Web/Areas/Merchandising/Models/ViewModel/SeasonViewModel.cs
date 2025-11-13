using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Merchandising.Models.ViewModel
{
    public class SeasonViewModel:OM_Season
    {
        public List<OM_Season> OmSeasons { get; set; }
        public SeasonViewModel()
        {
            OmSeasons=new List<OM_Season>();
        }
    }
}