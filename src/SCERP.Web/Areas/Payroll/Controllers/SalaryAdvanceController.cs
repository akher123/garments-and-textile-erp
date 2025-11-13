using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model.Custom;
using SCERP.Web.Areas.Payroll.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;
using SCERP.BLL.Manager.HRMManager;

namespace SCERP.Web.Areas.Payroll.Controllers
{
    public class SalaryAdvancesController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "salaryadvance-1,salaryadvance-2,salaryadvance-3")]
        public ActionResult Index(SalaryAdvanceViewModel model)
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
            }

            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            SalaryAdvance salaryAdvance = model;

            model.SearchFieldModel.SearchByEmployeeCardId = model.EmployeeCardId;
            model.SearchFieldModel.StartDate = model.FromDate;
            model.SearchFieldModel.EndDate = model.ToDate;

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.salaryAdvanceView = SalaryAdvanceManager.GetAllSalaryAdvancesByPaging(startPage, _pageSize, out totalRecords, salaryAdvance, model.SearchFieldModel) ?? new List<SalaryAdvanceView>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "salaryadvance-2,salaryadvance-3")]
        public ActionResult Edit(SalaryAdvanceViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.ReceivedDate = DateTime.Now;

                if (model.SalaryAdvanceId > 0)
                {
                    var salaryAdvance = SalaryAdvanceManager.GetSalaryAdvanceById(model.SalaryAdvanceId);

                    var employee = EmployeeManager.GetEmployeeById(salaryAdvance.EmployeeId);

                    if (employee != null)
                        model.EmployeeCardId = employee.EmployeeCardId;

                    model.EmployeeCardId = salaryAdvance.Employee.EmployeeCardId;
                    model.Amount = salaryAdvance.Amount;
                    model.ReceivedDate = salaryAdvance.ReceivedDate;
                    ViewBag.Title = "Edit Advance Amount";
                }
                else
                {
                    ViewBag.Title = "Add Advance Amount";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "salaryadvance-2,salaryadvance-3")]
        public ActionResult Save(SalaryAdvanceViewModel model)
        {
            var salaryAdvance = SalaryAdvanceManager.GetSalaryAdvanceById(model.SalaryAdvanceId) ?? new SalaryAdvance();

            var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
            if (employee == null)
                return ErrorResult("Employee Id is not valid !");

            if (model.Amount <= 0)
                return ErrorResult("Amount is not valid !");

            salaryAdvance.EmployeeId = employee.EmployeeId;
            salaryAdvance.ReceivedDate = model.ReceivedDate;
            salaryAdvance.Amount = model.Amount;

            var saveIndex = (model.SalaryAdvanceId > 0) ? SalaryAdvanceManager.EditSalaryAdvance(salaryAdvance) : SalaryAdvanceManager.SaveSalaryAdvance(salaryAdvance);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "salaryadvance-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var salaryAdvance = SalaryAdvanceManager.GetSalaryAdvanceById(id) ?? new SalaryAdvance();
            deleted = SalaryAdvanceManager.DeleteSalaryAdvance(salaryAdvance);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult EditBulkAdvance(SalaryAdvanceViewModel model)
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
                if (model.IsBulkSalaryAdvanceSearch)
                {
                    model.IsBulkSalaryAdvanceSearch = false;
                    return View(model);
                }

                model.EmployeesForAdvanceSalary = SalaryAdvanceManager.GetEmployeesForAdvanceSalary(model, model.SearchFieldModel);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult SaveBulkAdvance(List<SalaryAdvance> listSalaryAdvance)
        {
            var saveIndex = 0;
            try
            {

                if (listSalaryAdvance.Count > 0)
                {
                    saveIndex = SalaryAdvanceManager.SaveEmployeeSalaryAdvance(listSalaryAdvance);

                    if (saveIndex > 0)
                        return Json("Saved Successfully", JsonRequestBehavior.AllowGet);

                    return Json("Failed to Save!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return ErrorResult("Please select at least one Employee Id");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
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

        public JsonResult GetDepartmentLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DataSources = departmentLines }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new { Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines, }, JsonRequestBehavior.AllowGet);
        }
    }
}
