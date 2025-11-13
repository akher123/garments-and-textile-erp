using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class CityController : BaseController
    {
        [AjaxAuthorize(Roles = "city-1,city-2,city-3")]
        public ActionResult Index(CityViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.Cities = CityManager.GetCitiesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
         
        }
        [AjaxAuthorize(Roles = "city-2,city-3")]
        public ActionResult Edit(CityViewModel model)
        {
            ModelState.Clear();
            model.Countries = CountryManager.GetAllCountries();
            if (model.CityId > 0)
            {
                City city = CityManager.GetCityById(model.CityId);
                model.CityId = city.CityId;
                model.StateId = city.StateId;
                model.CountryId = city.CountryId;
                model.CityName = city.CityName;
                model.Latitude = city.Latitude;
                model.Longitude = city.Longitude;
            }
            return View(model);
        }
         [AjaxAuthorize(Roles = "city-2,city-3")]
        public ActionResult Save(City model)
        {
            try
            {
                var index = 0;
                index = model.CityId > 0 ? CityManager.EditCity(model) : CityManager.SaveCity(model);
                return index > 0 ? Reload() : ErrorResult("City save fail");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Systeme Error :" + exception.Message);
            }
        }
         [AjaxAuthorize(Roles = "city-3")]
        public ActionResult Delete(City model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = CityManager.DeleteCity(model.CityId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }

        public JsonResult GetCityByCountry(int countryId)
        {
            var cityList = CityManager.GetCityByCountry(countryId);
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
	}
}