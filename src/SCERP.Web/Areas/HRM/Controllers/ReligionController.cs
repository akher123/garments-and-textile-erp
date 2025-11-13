using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class ReligionController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "religion-1,religion-2,religion-3")]
        public ActionResult Index(ReligionViewModel model)
        {
            try
            {
                ModelState.Clear();
                Religion religion = model;
                religion.Name = model.SearchKey;
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
                model.Religions = ReligionManager.GetAllReligionsByPaging(startPage, _pageSize, out totalRecords, religion) ?? new List<Religion>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        [AjaxAuthorize(Roles = "religion-2,religion-3")]
        public ActionResult Edit(ReligionViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.ReligionId > 0)
                {
                    var religion = ReligionManager.GetReligionById(model.ReligionId);
                    model.Name = religion.Name;
                    model.NameInBengali = religion.NameInBengali;
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

        [AjaxAuthorize(Roles = "religion-2,religion-3")]
        public ActionResult Save(ReligionViewModel model)
        {
            var isExist = ReligionManager.CheckExistingReligion(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Religion already exist");
            }
            var religion = ReligionManager.GetReligionById(model.ReligionId) ?? new Religion();
            religion.Name = model.Name;
            religion.NameInBengali = model.NameInBengali;


            var saveIndex = (model.ReligionId > 0) ? ReligionManager.EditReligion(religion) : ReligionManager.SaveReligion(religion);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

        [AjaxAuthorize(Roles = "religion-3")]
        public ActionResult Delete(int religionId)
        {
            var deleted = 0;
            var religion = ReligionManager.GetReligionById(religionId) ?? new Religion();
            deleted = ReligionManager.DeleteReligion(religion);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "religion-1,religion-2,religion-3")]
        public void GetExcel(ReligionViewModel model)
        {
            var religions = ReligionManager.GetReligionBySearchKey(model.SearchKey);
            model.Religions = religions;

            const string fileName = "Religion";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Name", DataField = "Name"}
            };
            ReportConverter.CustomGridView(boundFields, model.Religions, fileName);
        }

        [AjaxAuthorize(Roles = "religion-1,religion-2,religion-3")]
        public ActionResult Print(ReligionViewModel model)
        {
            var religions = ReligionManager.GetReligionBySearchKey(model.SearchKey);
            model.Religions = religions;
            return View("_ReligionPdfReport", model);
        }

    }

}
