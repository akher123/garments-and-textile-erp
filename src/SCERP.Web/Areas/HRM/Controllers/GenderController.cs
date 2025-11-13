using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
namespace SCERP.Web.Areas.HRM.Controllers
{
    public class GenderController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "gender-1,gender-2,gender-3")]

        public ActionResult Index(GenderViewModel model)
        {
            try
            {
                ModelState.Clear();
                //if (!model.IsSearch)
                //{
                //    model.IsSearch = true;
                //    return View(model);
                //}
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.Title = model.SearchByTitle;
                model.Genders = GenderManager.GetGenders(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "gender-2,gender-3")]
        public ActionResult Edit(GenderViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.GenderId > 0)
                {
                    Gender genderObje = GenderManager.GetGenderById(model.GenderId);
                    model.Title = genderObje.Title;
                    model.TitleInBengali = genderObje.TitleInBengali;

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "gender-2,gender-3")]
        public ActionResult Save(Gender model)
        {
            var saveIndex = 0;
            bool isExist = GenderManager.IsExistGender(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.GenderId > 0 ? GenderManager.EditGender(model) : GenderManager.SaveGender(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("{0} Gender title already exist!", model.Title));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "gender-2,gender-3")]
        public ActionResult Delete(Gender gender)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = GenderManager.DeleteGender(gender.GenderId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

    }
}