using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SkillSetDifficultyController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "skillsetdifficulty-1,skillsetdifficulty-2,skillsetdifficulty-3")]
        public ActionResult Index(SkillSetDifficultyViewModel model)
        {
            try
            {
                ModelState.Clear();

                SkillSetDifficulty skillSetDifficulty = model;
                skillSetDifficulty.DifficultyName = model.SearchKey;
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

                int totalRecords = 0;
                model.SkillSetDifficulties = SkillSetDifficultyManager.GetAllSkillSetDifficultyByPaging(startPage, _pageSize, out totalRecords, skillSetDifficulty) ?? new List<SkillSetDifficulty>();
                model.TotalRecords = totalRecords;
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "skillsetdifficulty-2,skillsetdifficulty-3")]
        public ActionResult Edit(SkillSetDifficultyViewModel model)
        {

            ModelState.Clear();

            try
            {
                if (model.SkillSetDifficultyId > 0)
                {
                    var skillsetdifficulty = SkillSetDifficultyManager.GetSkillSetDifficultyById(model.SkillSetDifficultyId);
                    model.DifficultyName = skillsetdifficulty.DifficultyName;
                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.Title = "Add";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "skillsetdifficulty-2,skillsetdifficulty-3")]
        public ActionResult Save(SkillSetDifficultyViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = SkillSetDifficultyManager.CheckExistingSkillSetDifficulty(model);

                if (isExist)
                {
                    return ErrorResult(model.DifficultyName + " " + "Skill Difficulty already exist");
                }
                var skillsetdifficulty = SkillSetDifficultyManager.GetSkillSetDifficultyById(model.SkillSetDifficultyId) ?? new SkillSetDifficulty();
                skillsetdifficulty.DifficultyName = model.DifficultyName;
                saveIndex = (model.SkillSetDifficultyId > 0) ? SkillSetDifficultyManager.EditSkillSetDifficulty(skillsetdifficulty) : SkillSetDifficultyManager.SaveSkillSetDifficulty(skillsetdifficulty);
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "skillsetdifficulty-3")]
        public ActionResult Delete(int skillSetDifficultyId)
        {
            var deleted = 0;
            try
            {
                var skillsetdifficulty = SkillSetDifficultyManager.GetSkillSetDifficultyById(skillSetDifficultyId) ?? new SkillSetDifficulty();
                deleted = SkillSetDifficultyManager.DeleteSkillSetDifficulty(skillsetdifficulty);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}