using SCERP.Common;
using SCERP.Model.PayrollModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class PayrollProcessViewModel
    {
        public List<SelectModel> BonusRules { get; set; }
        public int BonusRuleId { get; set; }
        public PayrollProcessViewModel()
        {
            BonusRules = new List<SelectModel>();
        }

        public IEnumerable<SelectListItem> BonusRulesSelectListItem
        {
            get
            {
                return new SelectList(BonusRules, "Value", "Text");
            }
        }
    }
}