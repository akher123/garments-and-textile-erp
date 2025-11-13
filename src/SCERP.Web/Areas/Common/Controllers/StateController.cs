using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class StateController : BaseController
    {
         [AjaxAuthorize(Roles = "state-1,state-2,state-3")]
        public ActionResult Index(StateViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.States = StateManager.GetStatesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "state-2,state-3")]
        public ActionResult Edit(StateViewModel model)
        {
            ModelState.Clear();
            model.CountryList = CountryManager.GetAllCountries();
            if (model.StateId > 0)
            {
                var state = StateManager.GetStateById(model.StateId);
                model.StateId = state.StateId;
                model.CountryId = state.CountryId;
                model.StateName = state.StateName;
                model.Latitude = state.Latitude;
                model.Longitude = state.Longitude;
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "state-2,state-3")]
        public ActionResult Save(State model)
        {
            try
            {
                var index = 0;
                index = model.StateId > 0 ? StateManager.EditState(model) : StateManager.SaveState(model);
                return index > 0 ? Reload() : ErrorResult("State save fail");
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult("Systeme Error :" + exception.Message);
            }

        }
        [AjaxAuthorize(Roles = "state-3")]
        public ActionResult Delete(State model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = StateManager.DeleteState(model.StateId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }

        public JsonResult GetStateByCountry(int countryId)
        {
            var states=StateManager.GetStateByCountry(countryId);
            return Json(states,JsonRequestBehavior.AllowGet);
        }
    }
}