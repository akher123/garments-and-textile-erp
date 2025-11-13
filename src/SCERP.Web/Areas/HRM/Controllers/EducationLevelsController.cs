using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EducationLevelsController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "educationlevel-1,educationlevel-2,educationlevel-3")]
        public ActionResult Index(EducationLevelViewModel model)
        {

            ModelState.Clear();
            EducationLevel educationLevel = model;
            educationLevel.Title = model.SearchKey;

            //if (model.IsSearch)
            //{
            //    model.IsSearch = false;
            //    return View(model);
            //}

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.EducationLevels = EducationLevelManager.GetAllEducationLevelsByPaging(startPage, _pageSize, out totalRecords, educationLevel) ?? new List<EducationLevel>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "educationlevel-2,educationlevel-3")]
        public ActionResult Edit(EducationLevelViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var educationLevel = EducationLevelManager.GetEducationLevelById(model.Id);
                    model.Title = educationLevel.Title;
                    model.TitleInBengali = educationLevel.TitleInBengali;
                    model.Description = educationLevel.Description;
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


        [AjaxAuthorize(Roles = "educationlevel-2,educationlevel-3")]
        public ActionResult Save(EducationLevelViewModel model)
        {
            var isExist = EducationLevelManager.CheckExistingEducationLevel(model);

            if (isExist)
            {
                return ErrorResult(model.Title + " " + "Education Level already exist");
            }
            var educationLevel = EducationLevelManager.GetEducationLevelById(model.Id) ?? new EducationLevel();
            educationLevel.Title = model.Title;
            educationLevel.TitleInBengali = model.TitleInBengali;
            educationLevel.Description = model.Description;

            var saveIndex = (model.Id > 0) ? EducationLevelManager.EditEducationLevel(educationLevel) : EducationLevelManager.SaveEducationLevel(educationLevel);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }


        [AjaxAuthorize(Roles = "educationlevel-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var educationLevel = EducationLevelManager.GetEducationLevelById(id) ?? new EducationLevel();
            deleted = EducationLevelManager.DeleteEducationLevel(educationLevel);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "educationlevel-1,educationlevel-2,educationlevel-3")]
        public void GetExcel(EducationLevelViewModel model)
        {
            var educationLevels = EducationLevelManager.GetEducationLevelBySearchKey(model.SearchKey);
            model.EducationLevels = educationLevels;

            const string fileName = "EducationLevels";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Title", DataField = "Title"},
                new BoundField(){HeaderText = @"Description", DataField = "Description"}
            };
            ReportConverter.CustomGridView(boundFields, model.EducationLevels, fileName);
        }

        [AjaxAuthorize(Roles = "educationlevel-1,educationlevel-2,educationlevel-3")]
        public ActionResult Print(EducationLevelViewModel model)
        {
            var educationLevels = EducationLevelManager.GetEducationLevelBySearchKey(model.SearchKey);
            model.EducationLevels = educationLevels;

            return View("_EducationLevelPdfReport", model);
        }

    }

}
