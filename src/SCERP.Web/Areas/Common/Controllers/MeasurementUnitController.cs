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
    public class MeasurementUnitController : BaseController
    {

        [AjaxAuthorize(Roles = "measurementunit-1,measurementunit-2,measurementunit-3")]
        public ActionResult Index(MeasurementUnitViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            try
            {
                model.MeasurementUnits = MeasurementUnitManager.GetMeasurementUnitByPaging(model, out totalRecords);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "measurementunit-2,measurementunit-3")]
        public ActionResult Edit(MeasurementUnitViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.UnitId > 0)
                {
                    MeasurementUnit measurementUnit = MeasurementUnitManager.GetMeasurementUnitById(model.UnitId);
                    model.UnitName = measurementUnit.UnitName;
                    model.Description = measurementUnit.Description;

                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "measurementunit-2,measurementunit-3")]
        public ActionResult Save(MeasurementUnit model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.UnitId > 0 ? MeasurementUnitManager.EditMeasurementUnit(model) : MeasurementUnitManager.SaveMeasurementUnit(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult(String.Format("Failed to save {0}", model.UnitName));
        }

        [AjaxAuthorize(Roles = "measurementunit-3")]
        public ActionResult Delete(MeasurementUnit measurementUnit)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = MeasurementUnitManager.DeleteMeasurementUnit(measurementUnit.UnitId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete Brand");

        }

        public JsonResult CheckMeasurementUnit(MeasurementUnit model)
        {
            bool isExist = !MeasurementUnitManager.IsExistMeasurementUnit(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}