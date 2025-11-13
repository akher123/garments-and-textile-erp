using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class PayrollProcessController:BaseController
    {
        private readonly IPayrollProcessManager payrollProcessManager;
        private readonly IBonusRuleManager bonusRuleManager;
        public PayrollProcessController(IBonusRuleManager bonusRuleManager,IPayrollProcessManager payrollProcessManager)
        {
            this.bonusRuleManager = bonusRuleManager;
            this.payrollProcessManager = payrollProcessManager;
        }

        public ActionResult Index(PayrollProcessViewModel model)
        {
            model.BonusRules = bonusRuleManager.GetUnPorcessBonusRule(PortalContext.CurrentUser.CompanyId);
            return View(model);
        }

        public  ActionResult ProcessBonus(int bonusRuleId)
        {
            var bonusProcess = bonusRuleManager.GetBonusRuleById(bonusRuleId, PortalContext.CurrentUser.CompId);
            try
            {
                int processed = payrollProcessManager.ProcessBonus(bonusRuleId);
                if (bonusProcess.IsProcessed.Equals("Y"))
                {
                    return ErrorResult("Bonuse Already Processed");
                }
                else
                {
                    if (processed > 0)
                    {
                        return ErrorResult(String.Format("{0} Bonuse Processed Sucessfully ", bonusProcess.Title));
                    }
                  
                }
            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                
            }
            return ErrorResult(String.Format("{0} Bonuse Not Processed ", bonusProcess.Title));
        }
    }
}