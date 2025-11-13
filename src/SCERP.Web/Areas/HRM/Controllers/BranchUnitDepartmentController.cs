using System;
using System.Collections;
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
    public class BranchUnitDepartmentController : BaseController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "branchunitdepartment-1,branchunitdepartment-2,branchunitdepartment-3")]
        public ActionResult Index(BranchUnitDepartmentViewModel model)
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
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}
                model.Branches = BranchManager.GetAllBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);

                model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.UnitDepartments =
                UnitDepartmentManager.GetAllUnitDepatmeByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllBranchUnitDepartment(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunitdepartment-2,branchunitdepartment-3")]
        public ActionResult Edit(BranchUnitDepartmentViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                if (model.BranchUnitDepartmentId > 0)
                {
                    BranchUnitDepartment branchUnitDepartment = BranchUnitDepartmentManager.GetBranchUnitDepartmentById(model.BranchUnitDepartmentId);

                    model.BranchUnits = BranchUnitManager.GetBranchUnitByBranchId(branchUnitDepartment.BranchUnit.BranchId) as IEnumerable;
                    model.UnitDepartments =
                    UnitDepartmentManager.GetAllUnitDepatmeByBranchUnitId(branchUnitDepartment.BranchUnitId) as IEnumerable;

                    model.Branches =
                        BranchManager.GetAllBranchesByCompanyId(branchUnitDepartment.BranchUnit.Branch.CompanyId);
                    model.SerchCompanyId = branchUnitDepartment.BranchUnit.Branch.CompanyId;
                    model.SerchBranchId = branchUnitDepartment.BranchUnit.BranchId;

                    model.BranchUnitId = branchUnitDepartment.BranchUnitId;
                    model.UnitDepartmentId = branchUnitDepartment.UnitDepartmentId;
                    model.CreatedBy = branchUnitDepartment.CreatedBy;
                    model.EditedBy = branchUnitDepartment.EditedBy;
                    model.CreatedDate = branchUnitDepartment.CreatedDate;
                    model.EditedDate = branchUnitDepartment.EditedDate;
                    model.IsActive = branchUnitDepartment.IsActive;

                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "branchunitdepartment-2,branchunitdepartment-3")]
        public ActionResult Save(BranchUnitDepartment model)
        {
            var saveIndex = 0;

            bool isExist = BranchUnitDepartmentManager.IsExistBranchUnitDepartment(model);
            try
            {

                switch (isExist)
                {

                    case false:
                        {
                            saveIndex = model.BranchUnitDepartmentId > 0 ? BranchUnitDepartmentManager.EditranchUnitDepartment(model) : BranchUnitDepartmentManager.SaveBranchUnitDepartment(model);
                        }
                        break;
                    default:

                        return ErrorResult(string.Format("Branch Unit department already exist!"));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "branchunitdepartment-3")]
        public ActionResult Delete(BranchUnitDepartment model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = BranchUnitDepartmentManager.BranchUnitDepartment(model.BranchUnitDepartmentId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "branchunitdepartment-1,branchunitdepartment-2,branchunitdepartment-3")]
        public void GetExcel(BranchUnitDepartmentViewModel model)
        {
            try
            {
                model.SearchFieldModel.SearchByCompanyId = model.SerchCompanyId;
                model.SearchFieldModel.SearchByBranchId = model.SerchBranchId;
                model.SearchFieldModel.SearchByBranchUnitId = model.BranchUnitId;
                model.SearchFieldModel.SearchByUnitDepartmentId = model.UnitDepartmentId;
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentBySearchKey(model.SearchFieldModel);
                const string fileName = "BranchUnitDepartment";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "BranchUnit.Branch.Company.Name"},
                    new BoundField(){HeaderText = @"Branch",DataField = "BranchUnit.Branch.Name"},
                    new BoundField(){HeaderText = @"Unit",DataField = "UnitDepartment.Unit.Name"},
                      new BoundField(){HeaderText = @"Department",DataField = "UnitDepartment.Department.Name"},
                 };
                ReportConverter.CustomGridView(boundFields, model.BranchUnits, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "branchunitdepartment-1,branchunitdepartment-2,branchunitdepartment-3")]
        public ActionResult Print(BranchUnitDepartmentViewModel model)
        {
            try
            {
                model.SearchFieldModel.SearchByCompanyId = model.SerchCompanyId;
                model.SearchFieldModel.SearchByBranchId = model.SerchBranchId;
                model.SearchFieldModel.SearchByBranchUnitId = model.BranchUnitId;
                model.SearchFieldModel.SearchByUnitDepartmentId = model.UnitDepartmentId;
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentBySearchKey(model.SearchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_BranchUnitDepartmentReport", model);
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
            var unitDepartments = UnitDepartmentManager.GetAllUnitDepatmeByBranchUnitId(branchUnitId);
            return Json(new { Success = true, UnitDepartments = unitDepartments }, JsonRequestBehavior.AllowGet);
        }

    }
}