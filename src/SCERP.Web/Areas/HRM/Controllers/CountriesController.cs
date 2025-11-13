using System;
using System.Collections.Generic;

using System.Web.Helpers;
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
    public class CountriesController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "country-1,country-2,country-3")]
        public ActionResult Index(CountryViewModel model)
        {

            ModelState.Clear();
            Country country = model;
            country.CountryName = model.SearchKey;

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
            model.Countries = CountryManager.GetAllCountriesByPaging(startPage, _pageSize, out totalRecords, country) ?? new List<Country>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "country-2,country-3")]
        public ActionResult Edit(CountryViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.Id > 0)
                {
                    var country = CountryManager.GetCountryById(model.Id);
                    model.CountryName = country.CountryName;
                    model.CountryNameInBengali = country.CountryNameInBengali;
                    model.CountryCode = country.CountryCode;
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

        [AjaxAuthorize(Roles = "country-2,country-3")]
        public ActionResult Save(CountryViewModel model)
        {
            var isExist = CountryManager.CheckExistingCountry(model);
            if (isExist)
            {
                return ErrorResult(model.CountryName + " " + "Country already exist");
            }

            var country = CountryManager.GetCountryById(model.Id) ?? new Country();
            country.CountryName = model.CountryName;
            country.CountryNameInBengali = model.CountryNameInBengali;
            country.CountryCode = model.CountryCode;

            var saveIndex = (model.Id > 0) ? CountryManager.EditCountry(country) : CountryManager.SaveCountry(country);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "country-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var country = CountryManager.GetCountryById(id) ?? new Country();
            deleted = CountryManager.DeleteCountry(country);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "country-1,country-2,country-3")]
        public void GetExcel(CountryViewModel model)
        {
            List<Country> countries = CountryManager.GetCountryBySearchKey(model.SearchKey);
            model.Countries = countries;

            const string fileName = "Countries";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = "Country Name", DataField = "CountryName"},
                new BoundField(){HeaderText = "Country Code", DataField = "CountryCode"}
            };
            ReportConverter.CustomGridView(boundFields, model.Countries, fileName);
        }

        [AjaxAuthorize(Roles = "country-1,country-2,country-3")]
        public ActionResult Print(CountryViewModel model)
        {
            List<Country> countries = CountryManager.GetCountryBySearchKey(model.SearchKey);
            model.Countries = countries;


            return View("_CountryPdfReport", model);
        }


        public JsonResult OrigignAutocomplite(string originSerarcKey)
        {
            var countries=  CountryManager.GetCountryBySearchKey(originSerarcKey);
            return Json(countries, JsonRequestBehavior.AllowGet);
        }
    }
}
