using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SkillOperationController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "skilloperation-1,skilloperation-2,skilloperation-3")]
        public ActionResult Index(SkillOperationViewModel model)
        {
            try
            {
                ModelState.Clear();

                var skillsetdifficultylist = SkillSetDifficultyManager.GetAllSkillSetDifficulty();

                model.SkillSetDifficulties = skillsetdifficultylist;

                var skillsetcategorylist = SkillSetCategoryManager.GetAllSkillSetCategory();

                model.SkillSetCategories = skillsetcategorylist;


                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                var totalRecords = 0;
                model.SkillSetDifficultyId = model.SearchBySkillSetDifficultyId;
                model.CategoryId = model.SearchBySkillCategoryId;
                model.Name = model.SearchByOperationName;
                model.SkillOperations = SkillOperationManager.GetAllSkillOperationByPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "skilloperation-2,skilloperation-3")]
        public ActionResult Edit(SkillOperationViewModel model)
        {
            ModelState.Clear();
            try
            {
                var skillsetdifficulty = SkillSetDifficultyManager.GetAllSkillSetDifficulty();
                var skillsetcategories = SkillSetCategoryManager.GetAllSkillSetCategory();
                model.SkillSetCategories = skillsetcategories;
                model.SkillSetDifficulties = skillsetdifficulty;
                if (model.SkillOperationId > 0)
                {
                    var skillOperation = SkillOperationManager.GetSkillOperationById(model.SkillOperationId);
                    model.Name = skillOperation.Name;
                    model.CategoryId = skillOperation.CategoryId;
                    model.SkillSetDifficultyId = skillOperation.SkillSetDifficultyId;
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "skilloperation-2,skilloperation-3")]
        public ActionResult Save(SkillOperationViewModel model)
        {
            var saveIndex = 0;

            try
            {
                var isExist = SkillOperationManager.IsExistSkillOperation(model);
                if (isExist)
                {
                    return ErrorResult(model.Name + " " + "Skill operation already exist");
                }

                var skillOperation = SkillOperationManager.GetSkillOperationById(model.SkillOperationId) ?? new SkillOperation();
                skillOperation.Name = model.Name;

                skillOperation.SkillSetDifficultyId = model.SkillSetDifficultyId;
                skillOperation.CategoryId = model.CategoryId;

                saveIndex = (model.SkillOperationId > 0) ? SkillOperationManager.EditSkillOperation(skillOperation) : SkillOperationManager.SaveSkillOperation(skillOperation);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "skilloperation-3")]
        public ActionResult Delete(SkillOperation model)
        {
            var deleteIndex = 0;

            try
            {
                deleteIndex = SkillOperationManager.DeleteSkillOperation(model.SkillOperationId);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}