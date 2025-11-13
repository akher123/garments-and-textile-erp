using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class UnitController : BaseController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "unit-1,unit-2,unit-3")]
        public ActionResult Index(UnitViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                model.Name = model.SearchByName;
                model.Units = UnitManager.GetAllUnits(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "unit-2,unit-3")]
        public ActionResult Edit(UnitViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.UnitId > 0)
                {
                    var unit = UnitManager.GetUnitById(model.UnitId);
                    model.Name = unit.Name;
                    model.NameInBengali = unit.NameInBengali;
                    model.Description = unit.Description;
                    model.CreatedBy = unit.CreatedBy;
                    model.EditedBy = unit.EditedBy;
                    model.CreatedDate = unit.CreatedDate;
                    model.EditedDate = unit.EditedDate;
                    model.IsActive = unit.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "unit-2,unit-3")]
        public ActionResult Save(SCERP.Model.Unit model)
        {
            var saveIndex = 0;
            bool isExist = UnitManager.IsExistUnit(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.UnitId > 0 ? UnitManager.EditUnit(model) : UnitManager.SaveUnit(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("{0} Unit already exist!", model.Name));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "unit-3")]
        public ActionResult Delete(SCERP.Model.Unit unit)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = UnitManager.DeleteUnit(unit.UnitId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }

        [AjaxAuthorize(Roles = "unit-1,unit-2,unit-3")]
        public void GetExcel(UnitViewModel model)
        {
            try
            {
                model.Name = model.SearchByName;
                model.Units = UnitManager.GetAllUnitsBySearchKey(model);
                const string fileName = @"Unit List";
                var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Name",DataField = "Name"},
              new BoundField(){HeaderText = @"Unit NameInBengali",DataField = "NameInBengali"},
              new BoundField(){HeaderText = @"Description",DataField = "Description"},
   
            };
                ReportConverter.CustomGridView(boundFields, model.Units, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "unit-1,unit-2,unit-3")]
        public ActionResult Print(UnitViewModel model)
        {
            try
            {
                model.Name = model.SearchByName;
                model.Units = UnitManager.GetAllUnitsBySearchKey(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_UnitReport", model);
        }
    }
}