using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Planning;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class DailyLineLayoutViewModel : ProSearchModel<DailyLineLayoutViewModel>
    {
        public Dictionary<string, PLAN_DailyLineLayout> DailyLineLayouts { get; set; }
        public DateTime? OutputDate { get; set; }
        public DailyLineLayoutViewModel()
        {
            DailyLineLayouts=new Dictionary<string, PLAN_DailyLineLayout>();
  
        }
    }
}