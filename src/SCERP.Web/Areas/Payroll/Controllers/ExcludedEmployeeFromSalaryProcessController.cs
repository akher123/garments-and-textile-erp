using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class ExcludedEmployeeFromSalaryProcessController : BasePayrollController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "excludeemployee-1, excludeemployee-2, excludeemployee-3")]
        public ActionResult Index(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
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
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

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

                var totalRecords = 0;
                model.ExcludedEmployeeFromSalaryProcessRecords = ExcludedEmployeeFromSalaryProcessManager.GetExcludedEmployeeFromSalaryProcessInfo(startPage, _pageSize, model, model.SearchFieldModel, out totalRecords) ?? new List<PayrollExcludedEmployeeFromSalaryProcess>();
                model.TotalRecords = totalRecords;

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);

        }

        [AjaxAuthorize(Roles = "excludeemployee-2,excludeemployee-3")]
        public ActionResult Edit(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            ModelState.Clear();

            try
            {
                if (model.ExcludedEmployeeFromSalaryProcessId > 0)
                {
                    var excludedEmployeeFromSalaryProcess = ExcludedEmployeeFromSalaryProcessManager.GetExcludedEmployeeFromSalaryProcessById(model.ExcludedEmployeeFromSalaryProcessId);

                    model.EmployeeCardId = EmployeeManager.GetEmployeeById(excludedEmployeeFromSalaryProcess.EmployeeId).EmployeeCardId; // Must retrieve by Employee Guid Id
                    model.Year = excludedEmployeeFromSalaryProcess.Year;
                    model.Month = excludedEmployeeFromSalaryProcess.Month;
                    model.FromDate = excludedEmployeeFromSalaryProcess.FromDate;
                    model.ToDate = excludedEmployeeFromSalaryProcess.ToDate;
                    model.Remarks = excludedEmployeeFromSalaryProcess.Remarks;

                    ViewBag.Title = "Edit Exluded Employee";
                }
                else
                {
                    ViewBag.Title = "Add Exluded Employee";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "excludeemployee-3")]
        public ActionResult Delete(int ExcludedEmployeeFromSalaryProcessId)
        {
            var deleted = 0;
            var excludedEmployeeFromSalaryProcess = ExcludedEmployeeFromSalaryProcessManager.GetExcludedEmployeeFromSalaryProcessById(ExcludedEmployeeFromSalaryProcessId) ?? new ExcludedEmployeeFromSalaryProcessViewModel();
            excludedEmployeeFromSalaryProcess.IsActive = false;
            deleted = ExcludedEmployeeFromSalaryProcessManager.EditExcludedEmployeeFromSalaryProcess(excludedEmployeeFromSalaryProcess);
            return deleted > 0 ? Reload() : ErrorResult("Failed to delete!");
        }

        [AjaxAuthorize(Roles = "excludeemployee-2,excludeemployee-3")]
        public ActionResult Save(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            var excludedEmployeeFromSalaryProcessInfo = ExcludedEmployeeFromSalaryProcessManager.GetExcludedEmployeeFromSalaryProcessById(model.ExcludedEmployeeFromSalaryProcessId) ?? new PayrollExcludedEmployeeFromSalaryProcess();

            excludedEmployeeFromSalaryProcessInfo.EmployeeId = EmployeeManager.GetEmployeeIdByEmployeeCardId(model.EmployeeCardId);
            excludedEmployeeFromSalaryProcessInfo.EmployeeCardId = model.EmployeeCardId;
            excludedEmployeeFromSalaryProcessInfo.Year = model.Year;
            excludedEmployeeFromSalaryProcessInfo.Month = model.Month;
            excludedEmployeeFromSalaryProcessInfo.FromDate = model.FromDate;
            excludedEmployeeFromSalaryProcessInfo.ToDate = model.ToDate;
            excludedEmployeeFromSalaryProcessInfo.Remarks = model.Remarks;


            var saveIndex = (model.ExcludedEmployeeFromSalaryProcessId > 0) ? ExcludedEmployeeFromSalaryProcessManager.EditExcludedEmployeeFromSalaryProcess(excludedEmployeeFromSalaryProcessInfo) : ExcludedEmployeeFromSalaryProcessManager.SaveExcludedEmployeeFromSalaryProcess(excludedEmployeeFromSalaryProcessInfo);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");

        }

        [AjaxAuthorize(Roles = "excludeemployee-2,excludeemployee-3")]
        public ActionResult Exclude(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
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
                model.EmployeeTypes = EmployeeTypeManager.GetAllPermittedEmployeeTypes();

                if (!model.IsSearch)
                {
                    model.IsSearch = true;
                    return View(model);
                }

                model.EmployeesForExcludingFromSalryProcess = ExcludedEmployeeFromSalaryProcessManager.GetEmployeesForExcludingFromSalaryProcess(model.SearchFieldModel, model);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "excludeemployee-2,excludeemployee-3")]
        public ActionResult ProcessBulkEmployeeForExcludingFromSalaryProcess(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            try
            {
                var saveIndex = 0;
                model.SearchFieldModel.EmployeeIdList = new List<Guid>();
                model.SearchFieldModel.EmployeeIdList = model.EmployeeIdList;

                if (model.SearchFieldModel.EmployeeIdList.Count > 0)
                {
                    saveIndex = ExcludedEmployeeFromSalaryProcessManager.ProcessBulkEmployeesForExcludingFromSalaryProcess(model.SearchFieldModel, model);
                }
                else
                {
                    return ErrorResult("Please select any one for processing");
                }

                return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }


            return View(model);
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

        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
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

        public ActionResult GetEmployeeDetailByEmployeeCardID(ExludedEmployeeFromSalaryProcessInfoCustomModel model)
        {
            var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }

            var employeeDetails = EmployeeCompanyInfoManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            if (employeeDetails == null)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }
            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}