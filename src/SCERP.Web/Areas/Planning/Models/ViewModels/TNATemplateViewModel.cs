using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Planning;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class TNATemplateViewModel : PLAN_Activity
    {
        public TNATemplateViewModel()
        {
            Activities = new List<PLAN_Activity>();
            IsSearch = true;
        }

        public List<PLAN_Activity> Activities { get; set; }

        public int TemplateId { get; set; }
        public string OrderStyleRefId { get; set; }

        public List<OM_BuyOrdStyle> Styles { get; set; }

        public List<SelectListItem> StyleSelectListItem
        {
            get { return new SelectList(Styles, "OrderStyleRefId", "OrderNo").ToList(); }
        }
    }
}