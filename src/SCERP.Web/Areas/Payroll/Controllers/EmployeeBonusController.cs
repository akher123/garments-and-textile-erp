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
    public class EmployeeBonusController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "bonusprocess-1,bonusprocess-2,bonusprocess-3")]
        public ActionResult Index(EmployeeBonusViewModel model)
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
                model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
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

            EmployeeBonus employeeBonus = model;

            model.SearchFieldModel.SearchByEmployeeCardId = model.EmployeeCardId;
            model.SearchFieldModel.StartDate = model.FromDate;
            model.SearchFieldModel.EndDate = model.ToDate;

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.employeeBonusView = EmployeeBonusManager.GetAllEmployeeBonusesByPaging(startPage, _pageSize, out totalRecords, employeeBonus, model.SearchFieldModel) ?? new List<EmployeeBonusView>();
            model.TotalRecords = totalRecords;

            return View(model);
        }

        [AjaxAuthorize(Roles = "bonusprocess-2,bonusprocess-3")]
        public ActionResult Edit(EmployeeBonusViewModel model)
        {
            ModelState.Clear();

            try
            {
                model.EffectiveDate = DateTime.Now;

                if (model.EmployeeBonusId > 0)
                {
                    var employeeBonus = EmployeeBonusManager.GetEmployeeBonusById(model.EmployeeBonusId);

                    var employee = EmployeeManager.GetEmployeeById(employeeBonus.EmployeeId);

                    if (employee != null)
                        model.EmployeeCardId = employee.EmployeeCardId;

                    model.EmployeeCardId = employeeBonus.Employee.EmployeeCardId;
                    model.Amount = employeeBonus.Amount;
                    model.EffectiveDate = employeeBonus.EffectiveDate;
                    ViewBag.Title = "Edit Bonus";
                }
                else
                {
                    ViewBag.Title = "Add Bonus";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

         [AjaxAuthorize(Roles = "bonusprocess-2,bonusprocess-3")]
        public ActionResult Save(EmployeeBonusViewModel model)
        {
            var employeeBonus = EmployeeBonusManager.GetEmployeeBonusById(model.EmployeeBonusId) ?? new EmployeeBonus();

            var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
            if (employee == null)
                return ErrorResult("Employee Id is not valid !");

            if (model.Amount <= 0)
                return ErrorResult("Amount is not valid !");

            employeeBonus.EmployeeId = employee.EmployeeId;
            employeeBonus.EffectiveDate = model.EffectiveDate;
            employeeBonus.Amount = model.Amount;

            var saveIndex = (model.EmployeeBonusId > 0) ? EmployeeBonusManager.EditEmployeeBonus(employeeBonus) : EmployeeBonusManager.SaveEmployeeBonus(employeeBonus);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "bonusprocess-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var employeeBonus = EmployeeBonusManager.GetEmployeeBonusById(id) ?? new EmployeeBonus();
            deleted = EmployeeBonusManager.DeleteEmployeeBonus(employeeBonus);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "bonusprocess-2,bonusprocess-3")]
        public ActionResult EditBulkBonus(EmployeeBonusViewModel model)
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

                if (model.IsBulkBonusSearch)
                {
                    model.IsBulkBonusSearch = false;
                    return View(model);
                }

                model.EmployeesForBonus = EmployeeBonusManager.GetEmployeesForBonus(model, model.SearchFieldModel);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "bonusprocess-2,bonusprocess-3")]
        public ActionResult SaveBulkBonus(List<EmployeeBonus> listEmployeeBonuses)
        {
            var saveIndex = 0;
            try
            {

                if (listEmployeeBonuses.Count > 0)
                {
                    saveIndex = EmployeeBonusManager.SaveEmployeeBonus(listEmployeeBonuses);
                    return Json("Saved Successfully", JsonRequestBehavior.AllowGet);
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
