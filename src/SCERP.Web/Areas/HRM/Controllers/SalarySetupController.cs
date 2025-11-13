using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Areas.Payroll.Controllers;
using SCERP.Web.Controllers;
using System.Web.UI.WebControls;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class SalarySetupController : BasePayrollController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "salary-1,salary-2,salary-3")]
        public ActionResult Index(SalarySetupViewModel model)
        {
            ModelState.Clear();
            model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }
            model.EmployeeGrades = SalarySetupManager.GetEmpGradeByEmpType(model.SearchEmployeeTypeId);
            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords;
            model.EmployeeGrade.EmployeeTypeId = model.SearchEmployeeTypeId;
            model.EmployeeGradeId = model.SearchEmployeeGradeId;

            model.SalarySetup = SalarySetupManager.GetAllSalarySetupesByPaging(startPage, _pageSize, out totalRecords, model) ?? new List<SalarySetup>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "salary-2,salary-3")]
        public ActionResult Edit(SalarySetupViewModel model)
        {
            ModelState.Clear();
            model.EmployeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
            if (model.Id > 0)
            {
                var salarySetup = SalarySetupManager.GetSalarySetupById(model.Id);
                model.EmployeeGrade.EmployeeTypeId = salarySetup.EmployeeGrade.EmployeeTypeId;
                model.EmployeeGradeId = salarySetup.EmployeeGradeId;
                model.GrossSalary = salarySetup.GrossSalary;
                model.BasicSalary = salarySetup.BasicSalary;
                model.HouseRent = salarySetup.HouseRent;
                model.MedicalAllowance = salarySetup.MedicalAllowance;
                model.Conveyance = salarySetup.Conveyance;
                model.FoodAllowance = salarySetup.FoodAllowance;
                model.EntertainmentAllowance = salarySetup.EntertainmentAllowance;
                model.FromDate = salarySetup.FromDate;
                model.ToDate = salarySetup.ToDate;
                model.Description = salarySetup.Description;
                model.EmployeeGrades = SalarySetupManager.GetEmpGradeByEmpType(salarySetup.EmployeeGrade.EmployeeTypeId);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "salary-2,salary-3")]
        public ActionResult Save(SalarySetupViewModel model)
        {
            var saveIndex = 0;
            try
            {
                if (model.Is_GrossSalary_Equal_To_TotalSalary())
                {
                    using (var transactionScope = new TransactionScope())
                    {

                        if (model.Id > 0)
                        {
                            saveIndex = SalarySetupManager.EditSalarySetup(model);
                        }
                        else
                        {

                            var latestSalarySetupInfo = SalarySetupManager.GetLatestSalarySetupInfoByGrade(model);

                            if (latestSalarySetupInfo != null && model.FromDate != null)
                            {
                                if (latestSalarySetupInfo.FromDate > model.FromDate)
                                    return ErrorResult("Salary setup from this date already exists!");

                                latestSalarySetupInfo.ToDate = model.FromDate.Value.AddDays(-1);
                                SalarySetupManager.UpdateSalarySetupInfoDate(latestSalarySetupInfo);
                            }

                            saveIndex = SalarySetupManager.SaveSalarySetup(model);

                        }

                        transactionScope.Complete();
                    }
                }
                else
                {
                    return ErrorResult("Gross Salary doesn't equal to total salary");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "salary-3")]
        public ActionResult Delete(SalarySetup salarySetup)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = SalarySetupManager.DeleteSalarySetup(salarySetup);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public JsonResult GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(int employeeGradeId, int employeeTypeId)
        {

            var employeeGradeSalaryPercentage =
                 EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentangeByEmployeeGradeAndTypeId(
                     employeeGradeId, employeeTypeId);

            if (employeeGradeSalaryPercentage.EmployeeGradeId > 0)
            {
                return Json(new { employeeGradeSalaryPercentage = employeeGradeSalaryPercentage, Status = true, }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { employeeGradeSalaryPercentage = employeeGradeSalaryPercentage, Status = false, }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetEmployeeGradeSalaryPercentangeForBasicAndHouseRent(int employeeGradeId, int employeeTypeId, decimal grossSalary)
        {
            var employeeGradeSalaryPercentage = EmployeeGradeSalaryPercentageManager.GetEmployeeGradeSalaryPercentange(employeeGradeId, employeeTypeId, grossSalary);
            return Json(employeeGradeSalaryPercentage, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetAllGradeByTypeId(int id)
        {
            var grades = SalarySetupManager.GetEmpGradeByEmpType(id);
            return Json(new { Success = true, gradeList = grades }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpGradeByEmpType(int employeeTypeId)
        {
            var employeeGrade = SalarySetupManager.GetEmpGradeByEmpType(employeeTypeId);
            return Json(employeeGrade, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "salary-1,salary-2,salary-3")]
        public void GetExcel(SalarySetupViewModel model)
        {
            const string fileName = "SalarySetup";
            model.EmployeeGrade.EmployeeTypeId = model.SearchEmployeeTypeId;
            model.EmployeeGradeId = model.SearchEmployeeGradeId;
            model.SalarySetup = SalarySetupManager.GetAllSalarySetupNewBySearchKey(model);
            var boundFields = new List<BoundField>
            {
                new BoundField() {HeaderText = @"EmployeeType", DataField = "EmployeeGrade.EmployeeType.Title"},
                new BoundField() {HeaderText = @"EmployeeGrade", DataField = "EmployeeGrade.Name"},             
                new BoundField() {HeaderText = @"Gross Salary", DataField = "GrossSalary"},
                new BoundField() {HeaderText = @"Basic Salary", DataField = "BasicSalary"},
                new BoundField() {HeaderText = @"House Rent", DataField = "HouseRent"},
                new BoundField() {HeaderText = @"Medical Allowance", DataField = "MedicalAllowance"},
                new BoundField() {HeaderText = @"Conveyance", DataField = "Conveyance"},
                new BoundField() {HeaderText = @"Food Allowance", DataField = "FoodAllowance"},
                new BoundField() {HeaderText = @"Entertainment Allowance", DataField = "EntertainmentAllowance"},
                new BoundField() {HeaderText = @"From Date", DataField = "FromDate"},
                new BoundField() {HeaderText = @"To Date", DataField = "ToDate"},
                new BoundField() {HeaderText = @"Description", DataField = "Description"}
            };
            ReportConverter.CustomGridView(boundFields, model.SalarySetup, fileName);

        }

        [AjaxAuthorize(Roles = "salary-1,salary-2,salary-3")]
        public ActionResult Print(SalarySetupViewModel model)
        {
            const string fileName = "SalarySetup";
            model.EmployeeGrade.EmployeeTypeId = model.SearchEmployeeTypeId;
            model.EmployeeGradeId = model.SearchEmployeeGradeId;
            model.SalarySetup = SalarySetupManager.GetAllSalarySetupNewBySearchKey(model);
            return View("_SalarySetupPdfReport", model);
        }
    }
}
