using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Custom;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class DashBoardViewModel
    {
        public SpMisSewingProductionBoard SpMisSewingProductionBoard { get; set; }
        public Object SpMisDailyTagetVsAchivemnet { get; set; }
        public Object SpMisHourlyAchievement { get; set; }
        public List<Chart> Charts { get; set; }
        public string LastEntryDateTime { get; set; }
        public DashBoardViewModel()
        {
            Charts = new List<Chart>();
            SpMisSewingProductionBoard=new SpMisSewingProductionBoard();
        }
    }
}