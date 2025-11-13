using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SkillSetCategoryController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "skillsetcategory-1,skillsetcategory-2,skillsetcategory-3")]
        public ActionResult Index(SkillSetCategoryViewModel model)
        {
            try
            {
                ModelState.Clear();

                SkillSetCategory skillSetCategory = model;
                skillSetCategory.CategoryName = model.SearchKey;


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

                int totalRecords = 0;
                model.SkillSetCategories = SkillSetCategoryManager.GetAllSkillSetCategoryByPaging(startPage, _pageSize, out totalRecords, skillSetCategory) ?? new List<SkillSetCategory>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        [AjaxAuthorize(Roles = "skillsetcategory-2,skillsetcategory-3")]
        public ActionResult Edit(SkillSetCategoryViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.CategoryId > 0)
                {
                    var skillsetcategory = SkillSetCategoryManager.GetSkillSetCategoryById(model.CategoryId);
                    model.CategoryName = skillsetcategory.CategoryName;
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

        [AjaxAuthorize(Roles = "skillsetcategory-2,skillsetcategory-3")]
        public ActionResult Save(SkillSetCategoryViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = SkillSetCategoryManager.CheckExistingSkillSetCategory(model);
                if (isExist)
                {
                    return ErrorResult(model.CategoryName + " " + "Category already exist");
                }

                var skillsetcategory = SkillSetCategoryManager.GetSkillSetCategoryById(model.CategoryId) ?? new SkillSetCategory();
                skillsetcategory.CategoryName = model.CategoryName;
                saveIndex = (model.CategoryId > 0) ? SkillSetCategoryManager.EditSkillSetCategory(skillsetcategory) : SkillSetCategoryManager.SaveSkillSetCategory(skillsetcategory);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "skillsetcategory-3")]
        public ActionResult Delete(int categoryId)
        {
            var deleted = 0;
            try
            {
                var skillsetcategory = SkillSetCategoryManager.GetSkillSetCategoryById(categoryId) ?? new SkillSetCategory();
                deleted = SkillSetCategoryManager.DeleteSkillSetCategory(skillsetcategory);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

    }
}