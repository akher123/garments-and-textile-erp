using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EfficiencyRateController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "efficiencyrate-1,efficiencyrate-2,efficiencyrate-3")]
        public ActionResult Index(EfficiencyRateViewModel model)
        {

            try
            {
                ModelState.Clear();

                var skillOperationList = SkillOperationManager.GetAllSkillOperationManager();
                model.SkillOperations = skillOperationList;

                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;

                model.SkillOperationId = model.SearchBySkillOperationId;
                model.Rate = model.SearchByRate;

                model.EfficiencyRates = EfficiencyRateManager.GetAllEfficiencyByPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);

        }

        [AjaxAuthorize(Roles = "efficiencyrate-2,efficiencyrate-3")]
        public ActionResult Edit(EfficiencyRateViewModel model)
        {
            ModelState.Clear();
            try
            {
                var skilloperation = SkillOperationManager.GetAllSkillOperationManager();
                model.SkillOperations = skilloperation;
                if (model.EfficiencyRateId > 0)
                {
                    var efficiencyRate = EfficiencyRateManager.GetEficiencyRateById(model.EfficiencyRateId);
                    model.Rate = efficiencyRate.Rate;
                    model.SkillOperationId = efficiencyRate.SkillOperationId;
                    model.FromDate = efficiencyRate.FromDate;
                    model.ToDate = efficiencyRate.ToDate;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "efficiencyrate-2,efficiencyrate-3")]
        public ActionResult Save(EfficiencyRateViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = EfficiencyRateManager.IsExistEfficiencyRate(model);
                if (isExist)
                {
                    return ErrorResult(model.Rate + " " + "Efficiency Rate already exist");
                }

                var efficiencyRate = EfficiencyRateManager.GetEficiencyRateById(model.EfficiencyRateId) ?? new EfficiencyRate();
                efficiencyRate.Rate = model.Rate;

                efficiencyRate.SkillOperationId = model.SkillOperationId;
                efficiencyRate.FromDate = model.FromDate;
                efficiencyRate.ToDate = model.ToDate;

                saveIndex = (model.EfficiencyRateId > 0) ? EfficiencyRateManager.EditEfficiencyRate(efficiencyRate) : EfficiencyRateManager.SaveEfficiencyRate(efficiencyRate);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "efficiencyrate-3")]
        public ActionResult Delete(EfficiencyRate model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = EfficiencyRateManager.DeleteEfficiencyRate(model.EfficiencyRateId);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}