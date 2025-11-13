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
    public class EmployeeWorkShiftController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "employeeworkshift-1,employeeworkshift-2,employeeworkshift-3")]
        public ActionResult Index(EmployeeWorkShiftViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeWorkShiftDetails = EmployeeWorkShiftManager.GetAllAssignedEmployeeWorkShift(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
                return View(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);

        }

        [AjaxAuthorize(Roles = "employeeworkshift-2,employeeworkshift-3")]
        public ActionResult Create(EmployeeWorkShiftViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.WorkGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }

                if (model.CheckingBranchUnitWorkShiftId > 0)
                {
                    if (model.CheckingShiftDate == null)
                    return ErrorResult("Checking shift date required!");
                }

                if (model.CheckingShiftDate !=null)
                {
                    if (model.CheckingBranchUnitWorkShiftId == 0)
                        return ErrorResult("Checking work shift required!");
                }

                model.EmployeesForWorkShift = EmployeeWorkShiftManager.GetEmployeesForWorkShift(model,model.SearchFieldModel);

                return View(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkshift-2,employeeworkshift-3")]
        public ActionResult Save(EmployeeWorkShiftViewModel model)
        {
            try
            {
                var saveIndex = 0;

                if (model.EmployeeWorkShiftId > 0)
                {

                    var employeeWork = new EmployeeWorkShift
                    {
                        BranchUnitWorkShiftId = model.BranchUnitWorkShiftId,
                        EmployeeId = model.EmployeeId,
                        Remarks = model.Remarks,
                        EmployeeWorkShiftId = model.EmployeeWorkShiftId,
                    };
                    saveIndex = EmployeeWorkShiftManager.ChangeEmployeeWorkShifts(employeeWork);
                }
                else
                {
                    if (model.StartDate.HasValue && model.EndDate.HasValue && (model.EndDate.Value - model.StartDate.Value).Days >= 0)
                    {
                        var employeeWorkShifts = model.GetSelectedWorkShift();

                        foreach (var employeeWorkShift in employeeWorkShifts)
                        {
                            var isEmployeeWorkShiftExist = EmployeeWorkShiftManager.CheckEmployeeExistingWorkShift(employeeWorkShift);

                            if (isEmployeeWorkShiftExist)
                            {
                                return ErrorResult(string.Format("Error: Work shift already exist for an employee. Please, check and re-assign all!"));                                
                            }
                        }

                        if (model.EmployeeGuidIdList.Count > 0)
                        {
                            saveIndex = EmployeeWorkShiftManager.SaveEmployeeWorkShifts(employeeWorkShifts);
                        }
                        else
                        {
                            return ErrorResult("Please select at least one Employee Id");
                        }
                    }
                    else
                    {
                        return ErrorResult("From Date must be greater than To Date");
                    }
                }
                return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkshift-2,employeeworkshift-3")]
        public ActionResult Edit(EmployeeWorkShiftViewModel model)
        {
            try
            {

                model.VEmployeeWorkShiftDetail = EmployeeWorkShiftManager.GetEmployeeWorkshiftDetailById(model.EmployeeWorkShiftId);
                model.WorkShifts = model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.VEmployeeWorkShiftDetail.BranchUnitId);
                model.BranchUnitWorkShiftId = model.VEmployeeWorkShiftDetail.BranchUnitWorkShiftId;
                model.EmployeeId = model.VEmployeeWorkShiftDetail.EmployeeId;
                model.ShiftDate = model.VEmployeeWorkShiftDetail.ShiftDate;
                model.Remarks = model.VEmployeeWorkShiftDetail.Remarks;
                model.EmployeeWorkShiftId = model.VEmployeeWorkShiftDetail.EmployeeWorkShiftId;

            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "employeeworkshift-3")]
        public ActionResult Delete(EmployeeWorkShiftViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = EmployeeWorkShiftManager.DeleteEmployeeWorkShift(model.EmployeeWorkShiftId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "employeeworkshift-1,employeeworkshift-2,employeeworkshift-3")]
        public void GetExcel(SearchFieldModel searchField)
        {
            var model = new EmployeeWorkShiftViewModel();
            try
            {
                model.EmployeeWorkShiftDetails = EmployeeWorkShiftManager.GetEmployeeWorkShiftDetailBySearchKey(searchField);
                const string fileName = "EmployeeWorkShift";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Employee Name",DataField = "EmployeeName"},
                   new BoundField(){HeaderText = @"Employee ID",DataField = "EmployeeCardId"},
                   new BoundField(){HeaderText = @"WorkShift",DataField = "WorkShiftName"},
                   new BoundField(){HeaderText = @"Shift Date",DataField = "ShiftDate"},
                   new BoundField(){HeaderText = @"Designation",DataField = "Designation"},
                   new BoundField(){HeaderText = @"Company",DataField = "CompanyName"},
                   new BoundField(){HeaderText = @"Branch",DataField = "BranchName"}, 
                   new BoundField(){HeaderText = @"Unit",DataField = "UnitName"},
                   new BoundField(){HeaderText = @"DepartmentName",DataField = "DepartmentName"},
                   new BoundField(){HeaderText = @"Section",DataField = "SectionName"}, 
                   new BoundField(){HeaderText = @"Line",DataField = "LineName"},
                   
                 };
                ReportConverter.CustomGridView(boundFields, model.EmployeeWorkShiftDetails, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "employeeworkshift-1,employeeworkshift-2,employeeworkshift-3")]
        public ActionResult Print(SearchFieldModel searchField)
        {
            var model = new EmployeeWorkShiftViewModel();
            try
            {
                model.EmployeeWorkShiftDetails = EmployeeWorkShiftManager.GetEmployeeWorkShiftDetailBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_EmployeeWorkShiftReport", model);
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

        public ActionResult GetWorkShiftAndDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            var dataSources = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(branchUnitId);
            return Json(new { Success = true, DataSources = dataSources, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWorkGroupWorkShiftAndDepartmentByBranchUnitId(int branchUnitId)
        {
            var workGroups = WorkGroupManager.GetWorkGroupsByBranchUnitId(branchUnitId);
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            var branchUnitWorkShift = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(branchUnitId);
            return Json(new { Success = true, WorkGroups = workGroups, BranchUnitDepartments = branchUnitDepartments, BranchUnitWorkShifts = branchUnitWorkShift }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult WorkShitAssignIndex(EmployeeWorkShiftViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                
                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                model.WorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.EmployeeWorkShiftDetails = EmployeeWorkShiftManager.GetAllAssignedEmployeeWorkShift(startPage, 500000, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
                return View(model);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);

        }

        public ActionResult WorkShiftAssignSave(EmployeeWorkShiftViewModel model)
        {
            var saveIndex = 0;
            if (model.SearchFieldModel.SearchByBranchUnitId == 0)
            {
                return ErrorResult("Please select unit!");
            }
            try
            {
                
                

                if (model.WorkShiftIdList.Count > 0)
                {
                    saveIndex = EmployeeWorkShiftManager.UpdateWorkShiftQuick(model.WorkShiftIdList, model.BranchUnitWorkShiftId);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}