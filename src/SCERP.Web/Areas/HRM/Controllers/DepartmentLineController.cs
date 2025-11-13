using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class DepartmentLineController : BaseController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "departmentline-1,departmentline-2,departmentline-3")]
        public ActionResult Index(DepartmentLineViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Lines = LineManager.GetAllLines();
                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLine(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "departmentline-2,departmentline-3")]
        public ActionResult Edit(DepartmentLineViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Lines = LineManager.GetAllLines();
                if (model.DepartmentLineId > 0)
                {
                    DepartmentLine departmentLine = DepartmentLineManager.GetDepartmentLineById(model.DepartmentLineId);
                    model.SearchFieldModel.SearchByCompanyId = departmentLine.BranchUnitDepartment.BranchUnit.Branch.CompanyId;
                    model.SearchFieldModel.SearchByBranchId = departmentLine.BranchUnitDepartment.BranchUnit.BranchId;
                    model.SearchFieldModel.SearchByBranchUnitId = departmentLine.BranchUnitDepartment.BranchUnitId;
                    model.BranchUnitDepartmentId =
                        departmentLine.BranchUnitDepartmentId;
                    model.LineId = departmentLine.LineId;
                    model.CreatedBy = departmentLine.CreatedBy;
                    model.EditedBy = departmentLine.EditedBy;
                    model.CreatedDate = departmentLine.CreatedDate;
                    model.EditedDate = departmentLine.EditedDate;
                    model.IsActive = departmentLine.IsActive;

                    model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "departmentline-2,departmentline-3")]
        public ActionResult Save(DepartmentLineViewModel model)
        {
            var saveIndex = 0;
            bool isExist = DepartmentLineManager.IsExistDepartmentLine(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.DepartmentLineId > 0 ? DepartmentLineManager.EditranchDepartmentLine(model) : DepartmentLineManager.SaveDepartmentLine(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("Department line already exist!"));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "departmentline-1,departmentline-2,departmentline-3")]
        public void GetExcel(DepartmentLineViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineBySearchKey(model.SearchFieldModel);
                const string fileName = "DepartmentLine";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "BranchUnitDepartment.BranchUnit.Branch.Company.Name"},
                    new BoundField(){HeaderText = @"Branch",DataField = "BranchUnitDepartment.BranchUnit.Branch.Name"},
                    new BoundField(){HeaderText = @"Unit",DataField = "BranchUnitDepartment.UnitDepartment.Unit.Name"},
                      new BoundField(){HeaderText = @"Department",DataField = "BranchUnitDepartment.UnitDepartment.Department.Name"},
                         new BoundField(){HeaderText = @"Line",DataField = "Line.Name"},
                 };
                ReportConverter.CustomGridView(boundFields, model.DepartmentLines, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "departmentline-3")]
        public ActionResult Delete(DepartmentLineViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = DepartmentLineManager.DeleteDepartmentLine(model.DepartmentLineId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "departmentline-1,departmentline-2,departmentline-3")]
        public ActionResult Print(DepartmentLineViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineBySearchKey(model.SearchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_DepartmentLineReport", model);
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllBranchesByCompanyId(companyId);

            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetBranchUnitByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

    }
}