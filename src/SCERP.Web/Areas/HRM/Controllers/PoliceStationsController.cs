using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class PoliceStationsController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "policestation-1,policestation-2,policestation-3")]
        public ActionResult Index(PoliceStationViewModel model)
        {
            ModelState.Clear();
            var countryList = CountryManager.GetAllCountries();
            ViewBag.SearchByCountry = new SelectList(countryList, "Id", "CountryName");
            var countryId = model.SearchByCountry;
            var districtList = DistrictManager.GetDistrictsByCountry(countryId);
            ViewBag.SearchByDistrict = new SelectList(districtList, "Id", "Name", model.SearchByDistrict);
            PoliceStation policeStation = model;
            policeStation.CountryId = countryId;
            policeStation.DistrictId = model.SearchByDistrict;
            policeStation.Name = model.SearchByPoliceStation;
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
            model.PoliceStations = PoliceStationManager.GetAllPoliceStationsByPaging(startPage, _pageSize, policeStation, out totalRecords) ?? new List<PoliceStation>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public JsonResult GetDistrictByCountry(int countryId)
        {
            var districts = DistrictManager.GetDistrictsByCountry(countryId);
            return Json(new { Success = true, Districts = districts }, JsonRequestBehavior.AllowGet);
        }


        [AjaxAuthorize(Roles = "policestation-2,policestation-3")]
        public ActionResult Edit(PoliceStationViewModel model)
        {
            ModelState.Clear();
            try
            {
                var countries = CountryManager.GetAllCountries();

                if (model.Id > 0)
                {
                    var policeStation = PoliceStationManager.GetPoliceStationById(model.Id);

                    var districts = DistrictManager.GetDistrictsByCountry(policeStation.Country.Id);
                    ViewBag.CountryItemList = new SelectList(countries, "Id", "CountryName", policeStation.Country.Id);
                    ViewBag.DistrictItemList = new SelectList(districts, "Id", "Name", policeStation.District.Id);
                    model.Name = policeStation.Name;
                    model.NameInBengali = policeStation.NameInBengali;
                    ViewBag.Title = "Edit";
                }
                else
                {
                    ViewBag.CountryItemList = new SelectList(countries, "Id", "CountryName");
                    ViewBag.DistrictItemList = new SelectList(new List<District>(), "Id", "Name");
                    ViewBag.Title = "Add";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        [AjaxAuthorize(Roles = "policestation-2,policestation-3")]
        public ActionResult Save(PoliceStationViewModel model)
        {
            var isExist = PoliceStationManager.CheckExistingPoliceStation(model);

            if (isExist)
            {
                return ErrorResult(model.Name + " " + "Police Station already exist");
            }

            var policeStation = PoliceStationManager.GetPoliceStationById(model.Id) ?? new PoliceStation();
            policeStation.Name = model.Name;
            policeStation.NameInBengali = model.NameInBengali;
            policeStation.DistrictId = model.District.Id;
            policeStation.CountryId = model.Country.Id;

            var saveIndex = (model.Id > 0) ? PoliceStationManager.EditPoliceStation(policeStation) : PoliceStationManager.SavePoliceStation(policeStation);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }


        [AjaxAuthorize(Roles = "policestation-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var policeStation = PoliceStationManager.GetPoliceStationById(id) ?? new PoliceStation();
            deleted = PoliceStationManager.DeletePoliceStation(policeStation);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "policestation-1,policestation-2,policestation-3")]
        public void GetExcel(PoliceStationViewModel model)
        {
            List<PoliceStation> policeStations = PoliceStationManager.GetAllPoliceStationsBySearchKey(model.SearchByPoliceStation, model.SearchByDistrict, model.SearchByCountry);
            model.PoliceStations = policeStations;

            const string fileName = "PoliceStations";
            var boundFields = new List<BoundField>
            {
                new BoundField(){HeaderText = @"Police Station", DataField = "Name"},
                new BoundField(){HeaderText = @"District", DataField = "District.Name"},
                new BoundField(){HeaderText = @"Country", DataField = "Country.CountryName"},
            };
            ReportConverter.CustomGridView(boundFields, model.PoliceStations, fileName);
        }

        [AjaxAuthorize(Roles = "policestation-1,policestation-2,policestation-3")]
        public ActionResult Print(PoliceStationViewModel model)
        {
            List<PoliceStation> policeStations = PoliceStationManager.GetAllPoliceStationsBySearchKey(model.SearchByPoliceStation, model.SearchByDistrict, model.SearchByCountry);
            model.PoliceStations = policeStations;

            return View("_PoliceStationPdfReport", model);
        }


    }
}
