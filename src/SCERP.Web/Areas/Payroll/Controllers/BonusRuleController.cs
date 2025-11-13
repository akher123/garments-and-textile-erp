using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Model.PayrollModel;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Payroll.Controllers
{

    public class BonusRuleController : BaseController
    {
        private readonly IBonusRuleManager bonusRuleManager;
        public BonusRuleController(IBonusRuleManager bonusRuleManager)
        {
            this.bonusRuleManager = bonusRuleManager;
        }
        [AjaxAuthorize(Roles = "bonusrule-1,bonusrule-2,bonusrule-3")]
        public ActionResult Index(BonusRuleViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.BonusRules = bonusRuleManager.GetBonusRulesByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "bonusrule-2,bonusrule-3")]
        public ActionResult Edit(BonusRuleViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.BonusRule.BonusRuleId > 0)
                {
                    BonusRule bonusRule = bonusRuleManager.GetBonusRuleById(model.BonusRule.BonusRuleId, PortalContext.CurrentUser.CompId);
                    model.BonusRule = bonusRule;
                }
                else
                {
                    model.BonusRule.EffectiveDate = DateTime.Now;
                    model.BonusRule.BonusRoleRefId = bonusRuleManager.GetNewBounusRuleRefId(PortalContext.CurrentUser.CompId);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }

            return View(model);
        }

        [HttpPost]
        [AjaxAuthorize(Roles = "bonusrule-2,bonusrule-3")]
        public ActionResult Save(BonusRuleViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                if (bonusRuleManager.IsBounusRoleExist(model.BonusRule))
                {
                    return ErrorResult("Bonus Rule :" + model.BonusRule.Title + " " + "Already Exist ! Please Entry another Rule");
                }
                else
                {
                    index = model.BonusRule.BonusRuleId > 0 ? bonusRuleManager.EditBousRule(model.BonusRule) : bonusRuleManager.SaveBousBole(model.BonusRule);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Bonus Role :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit Hour !");
        }

        [HttpGet]
        [AjaxAuthorize(Roles = "bonusrule-3")]
        public ActionResult Delete(int bonusRoleId)
        {
            var index = 0;
            try
            {
                var bousRole=  bonusRuleManager.GetBonusRuleById(bonusRoleId, PortalContext.CurrentUser.CompId);
                index = bonusRuleManager.DeleteBounusRole(bousRole);
                if (bousRole.IsProcessed.Equals("Y"))// This Y=Processed and N=No Processed 
                {
                    return ErrorResult(string.Format("Could not possible to delete rule={0} because of it's already processed.", bousRole.Title));
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete Bonus role :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete Bonus Rule !");
        }
    }
}