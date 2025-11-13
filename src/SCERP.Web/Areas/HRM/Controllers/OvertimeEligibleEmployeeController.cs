using System;
using System.Collections.Generic;
using System.Linq;
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
    public class OvertimeEligibleEmployeeController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-1,overtimeeligibleemployee-2,overtimeeligibleemployee-3")]
        public ActionResult Index(OvertimeEligibleEmployeeViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.OvertimeEligibleEmployeeDetails = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployeeByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        public ActionResult IneligibleIndex(OvertimeEligibleEmployeeViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllPermittedCompanies();
                model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                if (model.IsSearch)
                {
                    model.IsSearch = false;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                int totalRecords = 0;
                model.OvertimeEligibleEmployeeDetails = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployeeByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-2,overtimeeligibleemployee-3")]
        public ActionResult Create(OvertimeEligibleEmployeeViewModel model)
        {
            ModelState.Clear();

            try
            {

                if (model.OvertimeEligibleEmployeeId > 0)
                {
                    var overtimeEligibleEmployee = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployee(model.OvertimeEligibleEmployeeId);
                    return View("Edit", overtimeEligibleEmployee);
                }
                else
                {
                    model.Companies = CompanyManager.GetAllPermittedCompanies();
                    model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                    model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                    model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                    if (model.IsAssigneeSerached)
                    {
                        model.IsAssigneeSerached = false;
                        return View(model);
                    }
                    var startPage = 0;
                    if (model.page.HasValue && model.page.Value > 0)
                    {
                        startPage = model.page.Value - 1;
                    }
                    var totalRecords = 0;
                    model.EmployeeCompanyInfos = OvertimeEligibleEmployeeManager.GetEmployes(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                    model.TotalRecords = totalRecords;
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult IneligibleCreate(OvertimeEligibleEmployeeViewModel model)
        {
            ModelState.Clear();

            try
            {

                if (model.OvertimeEligibleEmployeeId > 0)
                {
                    var overtimeEligibleEmployee = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployee(model.OvertimeEligibleEmployeeId);
                    return View("Edit", overtimeEligibleEmployee);
                }
                else
                {
                    model.Companies = CompanyManager.GetAllPermittedCompanies();
                    model.Branches = BranchManager.GetAllPermittedBranchesByCompanyId(model.SearchFieldModel.SearchByCompanyId);
                    model.BranchUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(model.SearchFieldModel.SearchByBranchId);
                    model.BranchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(model.SearchFieldModel.SearchByBranchUnitId);
                    model.DepartmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                    model.DepartmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(model.SearchFieldModel.SearchByBranchUnitDepartmentId);
                    if (model.IsAssigneeSerached)
                    {
                        model.IsAssigneeSerached = false;
                        return View(model);
                    }
                    var startPage = 0;
                    if (model.page.HasValue && model.page.Value > 0)
                    {
                        startPage = model.page.Value - 1;
                    }
                    var totalRecords = 0;
                    model.EmployeeCompanyInfos = OvertimeEligibleEmployeeManager.GetEmployes(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                    model.TotalRecords = totalRecords;
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-2,overtimeeligibleemployee-3")]
        public ActionResult Save(OvertimeEligibleEmployeeViewModel model)
        {
            try
            {
                var saveIndex = 0;

                if (model.OvertimeEligibleEmployeeId > 0)
                {
                    var overtimeEligibleEmployee = new OvertimeEligibleEmployee
                    {
                        OvertimeEligibleEmployeeId = model.OvertimeEligibleEmployeeId,
                        EmployeeId = model.EmployeeId,
                        OvertimeDate = model.OvertimeDate,
                        OvertimeHour = model.OvertimeHour,
                        Remarks = model.Remarks,
                        IsActive = model.IsActive,
                        Status = model.Status
                    };
                    saveIndex = OvertimeEligibleEmployeeManager.EditOvertimeEligibleEmployee(overtimeEligibleEmployee);
                }
                else
                {

                    if (model.ToOvertimeDate.HasValue && model.FromOvertimeDate.HasValue && (model.ToOvertimeDate.Value - model.FromOvertimeDate.Value).Days >= 0)
                    {
                        var assignedOvertimeEligibleEmployees = model.AssignedOvertimeEligibleEmployee();
                        if (assignedOvertimeEligibleEmployees.Count > 0)
                        {
                            saveIndex = OvertimeEligibleEmployeeManager.SaveOvertimeEligibleEmployee(assignedOvertimeEligibleEmployees);
                        }
                        else
                        {
                            return ErrorResult("Please select any one for assigne ");
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

        public ActionResult IneligibleSave(OvertimeEligibleEmployeeViewModel model)
        {
            try
            {
                var saveIndex = 0;

                if (model.OvertimeEligibleEmployeeId > 0)
                {
                    var overtimeEligibleEmployee = new OvertimeEligibleEmployee
                    {
                        OvertimeEligibleEmployeeId = model.OvertimeEligibleEmployeeId,
                        EmployeeId = model.EmployeeId,
                        OvertimeDate = model.OvertimeDate,
                        OvertimeHour = model.OvertimeHour,
                        Remarks = model.Remarks,
                        IsActive = model.IsActive,
                        Status = model.Status
                    };
                    saveIndex = OvertimeEligibleEmployeeManager.EditOvertimeEligibleEmployee(overtimeEligibleEmployee);
                }
                else
                {
                    model.ToOvertimeDate = model.FromOvertimeDate;
                    if (model.ToOvertimeDate.HasValue && model.FromOvertimeDate.HasValue && (model.ToOvertimeDate.Value - model.FromOvertimeDate.Value).Days >= 0)
                    {

                        foreach (var item in model.EmployeeGuidIdList)
                        {
                            var overtimeEligibleEmployee = new OvertimeEligibleEmployee
                            {

                                EmployeeId = item,
                                OvertimeDate = model.FromOvertimeDate.Value

                            };
                            List<OvertimeEligibleEmployee> overtimeEligibleEmployeeget =
                            OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployeeListByEmpIdAndDate(
                                overtimeEligibleEmployee);
                            
                            foreach (var item2 in overtimeEligibleEmployeeget)
                            {
                                if (item2.IsActive)
                                {
                                    item2.IsActive = false;
                                    saveIndex += OvertimeEligibleEmployeeManager.EditOvertimeEligibleEmployee(item2);
                                }
                                
                            }
                            
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

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-1,overtimeeligibleemployee-2,overtimeeligibleemployee-3")]
        public void GetExcel(SearchFieldModel searchField)
        {
            var model = new OvertimeEligibleEmployeeViewModel();
            try
            {
                model.OvertimeEligibleEmployeeDetails = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployeeBySearchKey(searchField);
                const string fileName = "OvertimeEligibleEmployee";
                var boundFields = new List<BoundField>
                 {
                    new BoundField(){HeaderText = @"Employee Name",DataField = "EmployeeName"},
                    new BoundField(){HeaderText = @"Employee ID",DataField = "EmployeeCardId"},
                    new BoundField(){HeaderText = @"OT Date",DataField = "OvertimeDate"},
                   new BoundField(){HeaderText = @"OT(Hours)",DataField = "OvertimeHour"},
                   new BoundField(){HeaderText = @"Remarks",DataField = "Remarks"}, 
                       new BoundField(){HeaderText = @"Designation",DataField = "Designation"},
                   new BoundField(){HeaderText = @"Company",DataField = "CompanyName"},
                   new BoundField(){HeaderText = @"Branch",DataField = "BranchName"}, 
                       new BoundField(){HeaderText = @"Unit",DataField = "UnitName"},
                   new BoundField(){HeaderText = @"DepartmentName",DataField = "DepartmentName"},
                   new BoundField(){HeaderText = @"Section",DataField = "SectionName"}, 
                     new BoundField(){HeaderText = @"Line",DataField = "LineName"},
                 };
                ReportConverter.CustomGridView(boundFields, model.OvertimeEligibleEmployeeDetails, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-1,overtimeeligibleemployee-2,overtimeeligibleemployee-3")]
        public ActionResult Print(SearchFieldModel searchField)
        {
            var model = new OvertimeEligibleEmployeeViewModel();
            try
            {

                model.OvertimeEligibleEmployeeDetails = OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployeeBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_OvertimeEligibleEmployeeReport", model);
        }

        [AjaxAuthorize(Roles = "overtimeeligibleemployee-3")]
        public ActionResult Delete(OvertimeEligibleEmployeeViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                OvertimeEligibleEmployee overtimeEligibleEmployee =
                    OvertimeEligibleEmployeeManager.GetOvertimeEligibleEmployee(model.OvertimeEligibleEmployeeId);
                deleteIndex = OvertimeEligibleEmployeeManager.DeleteOvertimeEligibleEmployee(overtimeEligibleEmployee);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
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

        public JsonResult GetAllUnitDepatmeByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new { Success = true, BranchUnitDepartments = branchUnitDepartments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
    }
}