using SCERP.Model.PayrollModel;
using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class BonusRuleViewModel : ProSearchModel<BonusRule>
    {
        public BonusRuleViewModel()
        {
            BonusRule = new BonusRule();
            BonusRules = new List<BonusRule>();
        }
        public BonusRule BonusRule { get; set; }
        public List<BonusRule> BonusRules { get; set; }

    }
}