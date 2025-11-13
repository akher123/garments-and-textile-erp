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
    public class UnitDepartmentController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "unitdepartment-1,unitdepartment-2,unitdepartment-3")]
        public ActionResult Index(UnitDepartmentViewModel model)
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

                model.Units = UnitManager.GetAllUnits();
                model.Departments = DepartmentManager.GetAllDepartments();

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                model.UnitId = model.SearchByUnitId;
                model.DepartmentId = model.SearchByDepartmentId;

                model.UnitDepartments = UnitDepartmentManager.GetAllUnitDepartmentsByPaging(startPage, _pageSize, model, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "unitdepartment-2,unitdepartment-3")]
        public ActionResult Edit(UnitDepartmentViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Units = UnitManager.GetAllUnits();
                model.Departments = DepartmentManager.GetAllDepartments();

                if (model.UnitDepartmentId > 0)
                {
                    var unitDepartment = UnitDepartmentManager.GetUnitDepartmentById(model.UnitDepartmentId);
                    model.UnitId = unitDepartment.UnitId;
                    model.DepartmentId = unitDepartment.DepartmentId;
                    model.CreatedDate = unitDepartment.CreatedDate;
                    model.CreatedBy = unitDepartment.CreatedBy;
                    model.EditedDate = unitDepartment.EditedDate;
                    model.EditedBy = unitDepartment.EditedBy;
                    model.IsActive = unitDepartment.IsActive;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "unitdepartment-2,unitdepartment-3")]
        public ActionResult Save(UnitDepartment model)
        {
            var saveIndex = 0;
            var isExist = UnitDepartmentManager.IsExistUnitDepartment(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.UnitDepartmentId > 0 ? UnitDepartmentManager.EditUnitDepartment(model) : UnitDepartmentManager.SaveUnitDepartment(model);
                        }
                        break;
                    default:
                        return ErrorResult("Unit Department already exist");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "unitdepartment-3")]
        public ActionResult Delete(UnitDepartment unitDepartment)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = UnitDepartmentManager.DeleteUnitDepartment(unitDepartment.UnitDepartmentId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");

        }


        [AjaxAuthorize(Roles = "unitdepartment-1,unitdepartment-2,unitdepartment-3")]
        public void GetExcel(UnitDepartmentViewModel model)
        {
            try
            {
                model.UnitId = model.SearchByUnitId;
                model.DepartmentId = model.SearchByDepartmentId;

                model.UnitDepartments = UnitDepartmentManager.GetAllUnitDepartmentsBySearchKey(model);
                const string fileName = "UnitDepartment";
                var boundFields = new List<BoundField>
                {
                    new BoundField(){HeaderText = @"Unit Name",DataField = "Unit.Name"},
                    new BoundField(){HeaderText = @"Department Name",DataField = "Department.Name"},
                };

                ReportConverter.CustomGridView(boundFields, model.UnitDepartments, fileName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
        }


        [AjaxAuthorize(Roles = "unitdepartment-1,unitdepartment-2,unitdepartment-3")]
        public ActionResult Print(UnitDepartmentViewModel model)
        {
            try
            {
                model.UnitId = model.SearchByUnitId;
                model.DepartmentId = model.SearchByDepartmentId;
                model.UnitDepartments = UnitDepartmentManager.GetAllUnitDepartmentsBySearchKey(model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return PartialView("_UnitDepartmentReport", model);
        }

    }
}