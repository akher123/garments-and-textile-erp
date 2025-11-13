using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class DepartmentSectionController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "departmentsection-1,departmentsection-2,departmentsection-3")]
        public ActionResult Index(DepartmentSectionViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Sections = SectionManager.GetListOfSection();
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
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "departmentsection-2,departmentsection-3")]
        public ActionResult Edit(DepartmentSectionViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.Sections = SectionManager.GetListOfSection();
                if (model.DepartmentSectionId > 0)
                {
                    DepartmentSection departmentSection = DepartmentSectionManager.GetDepartmentSectionById(model.DepartmentSectionId);
                    model.SearchFieldModel.SearchByCompanyId = departmentSection.BranchUnitDepartment.BranchUnit.Branch.CompanyId;
                    model.SearchFieldModel.SearchByBranchId = departmentSection.BranchUnitDepartment.BranchUnit.BranchId;
                    model.SearchFieldModel.SearchByBranchUnitId = departmentSection.BranchUnitDepartment.BranchUnitId;
                    model.BranchUnitDepartmentId = departmentSection.BranchUnitDepartmentId;
                    model.SectionId = departmentSection.SectionId;
                    model.CreatedBy = departmentSection.CreatedBy;
                    model.EditedBy = departmentSection.EditedBy;
                    model.CreatedDate = departmentSection.CreatedDate;
                    model.EditedDate = departmentSection.EditedDate;
                    model.IsActive = departmentSection.IsActive;
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

        [AjaxAuthorize(Roles = "departmentsection-2,departmentsection-3")]
        public ActionResult Save(DepartmentSectionViewModel model)
        {
            var saveIndex = 0;
            bool isExist = DepartmentSectionManager.IsExistDepartmentSection(model);
            try
            {
                switch (isExist)
                {
                    case false:
                        {
                            saveIndex = model.DepartmentSectionId > 0 ? DepartmentSectionManager.EditDepartmentSection(model) : DepartmentSectionManager.SaveDepartmentSection(model);
                        }
                        break;
                    default:
                        return ErrorResult(string.Format("Department section already exist!"));
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "departmentsection-3")]
        public ActionResult Delete(DepartmentSectionViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = DepartmentSectionManager.DeleteDepartmentSection(model.DepartmentSectionId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "departmentsection-1,departmentsection-2,departmentsection-3")]
        public void GetExcel(DepartmentSectionViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionBySearchKey(model.SearchFieldModel);
                const string fileName = "DepartmentSection";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Company",DataField = "BranchUnitDepartment.BranchUnit.Branch.Company.Name"},
                    new BoundField(){HeaderText = @"Branch",DataField = "BranchUnitDepartment.BranchUnit.Branch.Name"},
                    new BoundField(){HeaderText = @"Unit",DataField = "BranchUnitDepartment.UnitDepartment.Unit.Name"},
                      new BoundField(){HeaderText = @"Department",DataField = "BranchUnitDepartment.UnitDepartment.Department.Name"},
                         new BoundField(){HeaderText = @"Section",DataField = "Section.Name"},
                 };
                ReportConverter.CustomGridView(boundFields, model.DepartmentSections, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }


        [AjaxAuthorize(Roles = "departmentsection-1,departmentsection-2,departmentsection-3")]
        public ActionResult Print(DepartmentSectionViewModel model, SearchFieldModel searchField)
        {
            try
            {
                model.SearchFieldModel = searchField;
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionBySearchKey(model.SearchFieldModel);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_DepartmentSectionReport", model);
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
            var unitDepartments = BranchUnitDepartmentManager.GetBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, UnitDepartments = unitDepartments }, JsonRequestBehavior.AllowGet);
        }

    }
}