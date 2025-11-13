using System;
using System.Collections.Generic;

using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.HRM.Controllers
{
    public class DistrictsController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "district-1,district-2,district-3")]
        public ActionResult Index(DistrictViewModel model)
        {

            ModelState.Clear();

            var countryList = CountryManager.GetAllCountries();
            ViewBag.SearchByCountry = new SelectList(countryList, "Id", "CountryName");

            District district = model;
            district.Name = model.SearchByDistrict;
            district.CountryId = model.SearchByCountry;

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

            var totalRecords = 0;
            model.Districts = DistrictManager.GetAllDistrictsByPaging(startPage, _pageSize, out totalRecords, district) ?? new List<District>();
            model.TotalRecords = totalRecords;

            return View(model);
        }
           
        [AjaxAuthorize(Roles = "district-2,district-3")]
        public ActionResult Edit(DistrictViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.Countries = CountryManager.GetAllCountries();

                if (model.Id > 0)
                {
                    var district = DistrictManager.GetDistrictById(model.Id);
                    model.Name = district.Name;
                    model.NameInBengali = district.NameInBengali;                   
                    model.CountryId = district.CountryId;
                    ViewBag.Title = "Edit District";
                }
                else
                {
                    ViewBag.Title = "Add District";
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "district-2,district-3")]
        public ActionResult Save(DistrictViewModel model)
        {
            var isExist = DistrictManager.CheckExistingDistrict(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "District already exist");
            }
            var district = DistrictManager.GetDistrictById(model.Id) ?? new District();
            district.CountryId = model.CountryId;
            district.Name = model.Name;
            district.NameInBengali = model.NameInBengali;

            var saveIndex = (model.Id > 0) ? DistrictManager.EditDistrict(district) : DistrictManager.SaveDistrict(district);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }


        [AjaxAuthorize(Roles = "district-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var district = DistrictManager.GetDistrictById(id) ?? new District();
            deleted = DistrictManager.DeleteDistrict(district);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public JsonResult GetPoliceStationByDistrict(int districtId)
        {
            List<PoliceStation> policeStationsByDistrict = null;
            try
            {
                policeStationsByDistrict = PoliceStationManager.GetPoliceStationByDistrict(districtId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                policeStationsByDistrict = null;
            }
            return Json(policeStationsByDistrict, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "district-1,district-2,district-3")]
        public void GetExcel(DistrictViewModel model)
        {
            List<District> districts = DistrictManager.GetDistrictBySearchKey(model.SearchByCountry, model.SearchByDistrict);
            model.Districts = districts;

            const string fileName = "Districts";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = "District", DataField = "Name"},
                new BoundField(){HeaderText = "Country", DataField = "Country.CountryName"}
            };
            ReportConverter.CustomGridView(boundFields, model.Districts, fileName);
        }

        [AjaxAuthorize(Roles = "district-1,district-2,district-3")]
        public ActionResult Print(DistrictViewModel model)
        {
            List<District> districts = DistrictManager.GetDistrictBySearchKey(model.SearchByCountry, model.SearchByDistrict);
            model.Districts = districts;

            return View("_DistrictPdfReport", model);
        }
    }
}
