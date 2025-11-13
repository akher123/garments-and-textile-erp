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
    public class EmployeeWorkGroupController : BaseHrmController
    {
        private readonly int _pageSize = 10; //AppConfig.PageSize;

        [AjaxAuthorize(Roles = "employeeworkgroup-1,employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Index(EmployeeWorkGroupViewModel model)
        {
            ModelState.Clear();
            model.Companies = CompanyManager.GetAllPermittedCompanies();
            model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
            model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
            model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
            model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
            model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
            model.WorkGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);

            if (!model.IsSearch)
            {
                model.IsSearch = true;

                return View(model);
            }

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }
            var totalRecords = 0;

            model.VEmployeeWorkGroupDetails = EmployeeWorkGroupManager.GetAllEmployeeWorkGroupByPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Create(EmployeeWorkGroupViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.WorkGroupId = model.SearchFieldModel.SearchByWorkGroupId;
                model.WorkGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;

                model.EmployeeCompanyInfos = EmployeeWorkGroupManager.GetAllUnAssignedEmployeeWorkGroup(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Edit(EmployeeWorkGroupViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.WorkGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.WorkGroupId = model.SearchFieldModel.SearchByWorkGroupId;
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                if (!model.IsSearch)
                {
                    model.IsSearch = true;

                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.VEmployeeWorkGroupDetails = EmployeeWorkGroupManager.GetAllEmployeeWorkGroupByPaging(startPage, _pageSize, out totalRecords, model.SearchFieldModel, model);
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Save(EmployeeWorkGroupViewModel model)
        {
            try
            {
                var saveIndex = 0;
                List<EmployeeWorkGroup> employeeWorkGroups;
                if (model.EmployeeGuidIdList.Count > 0)
                {
                    employeeWorkGroups = model.GetSelectedEmployeeWorkGroups();
                    saveIndex = EmployeeWorkGroupManager.SaveEmployeeWorkGroups(employeeWorkGroups);
                }
                else if (model.EmployeeWorkGroupIdList.Count > 0)
                {
                    employeeWorkGroups = model.GetEditedEmployeeWorkGroups();
                    saveIndex = EmployeeWorkGroupManager.EditEmployeeWorkGroups(employeeWorkGroups);
                }
                else
                {
                    return ErrorResult("Please select any one for assigne ");
                }
                return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult SaveForEdit(EmployeeWorkGroupViewModel model)
        {
            {
                try
                {
                    var saveIndex = 0;
                    var employeeWorkGroups = model.GetEditedEmployeeWorkGroups();
                    if (employeeWorkGroups.Count > 0)
                    {

                        saveIndex = EmployeeWorkGroupManager.SaveEmployeeWorkGroups(employeeWorkGroups);
                    }
                    else
                    {
                        return ErrorResult("Please select any one for assigne ");
                    }

                    return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
                }
                catch (Exception exception)
                {
                    Errorlog.WriteLog(exception);
                }


                return View(model);
            }
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-3")]
        public ActionResult Delete(int? EmployeeWorkGroupId)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = EmployeeWorkGroupManager.DeleteEmployeeWorkGroup(EmployeeWorkGroupId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "employeeworkgroup-1,employeeworkgroup-2,employeeworkgroup-3")]
        public void GetExcel(SearchFieldModel searchField)
        {
            var model = new EmployeeWorkGroupViewModel();
            try
            {
                model.VEmployeeWorkGroupDetails = EmployeeWorkGroupManager.GetEmployeeWorkGroupDetailBySearchKey(searchField);
                const string fileName = "EmployeeWorkGroup";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Employee Name",DataField = "EmployeeName"},
                   new BoundField(){HeaderText = @"Employee ID",DataField = "EmployeeCardId"},
                   new BoundField(){HeaderText = @"WorkGroup",DataField = "WorkGroupName"},
                   new BoundField(){HeaderText = @"Assigned Date",DataField = "AssignedDate"},
                   new BoundField(){HeaderText = @"Designation",DataField = "Designation"},
                   new BoundField(){HeaderText = @"Company",DataField = "CompanyName"},
                   new BoundField(){HeaderText = @"Branch",DataField = "BranchName"}, 
                   new BoundField(){HeaderText = @"Unit",DataField = "UnitName"},
                   new BoundField(){HeaderText = @"DepartmentName",DataField = "DepartmentName"},
                   new BoundField(){HeaderText = @"Section",DataField = "SectionName"}, 
                   new BoundField(){HeaderText = @"Line",DataField = "LineName"},
                   
                 };
                ReportConverter.CustomGridView(boundFields, model.VEmployeeWorkGroupDetails, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "employeeworkgroup-1,employeeworkgroup-2,employeeworkgroup-3")]
        public ActionResult Print(SearchFieldModel searchField)
        {
            var model = new EmployeeWorkGroupViewModel();
            try
            {

                model.VEmployeeWorkGroupDetails = EmployeeWorkGroupManager.GetEmployeeWorkGroupDetailBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_EmployeeWorkGroupEmployeeReport", model);
        }

        public ActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branches = BranchManager.GetAllPermittedBranchesByCompanyId(companyId);
            return Json(new { Success = true, Branches = branches }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new { Success = true, BrancheUnits = brancheUnits }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWorkGroupByBranchUnitId(int branchUnitId)
        {
            var workGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(branchUnitId);
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, WorkGroups = workGroups, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
           
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
    }

}