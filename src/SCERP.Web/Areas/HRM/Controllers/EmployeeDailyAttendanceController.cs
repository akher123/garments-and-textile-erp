using System;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeDailyAttendanceController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "dailyattendance-1,dailyattendance-2,dailyattendance-3")]
        public ActionResult Index(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
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

                var statusList = from StatusValue status in Enum.GetValues(typeof (StatusValue))
                    select new {Id = (int) status, Name = status.ToString()};
                ViewBag.EmployeeStatus = new SelectList(statusList, "Id", "Name");


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
                model.EmployeeDailyAttendances = EmployeeDailyAttendanceManager.GetEmployeeDailyAttendanceByPaging(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);

                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "dailyattendance-2,dailyattendance-3")]
        public ActionResult Edit(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            ModelState.Clear();
            model.EmployeeCompanyInfo = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(model.SearchFieldModel.SearchByEmployeeCardId);
            model.Date = DateTime.Now;

            if (model.EmployeeCompanyInfo != null)
            {
                model.EmployeeCardId = model.EmployeeCardId;
                model.EmployeeId = model.EmployeeCompanyInfo.EmployeeId;
            }

            if (model.Id > 0)
            {
                EmployeeDailyAttendance employeeDailyAttendance = EmployeeDailyAttendanceManager.GetEmployeeDailyAttendance(model.Id);
                model.EmployeeCompanyInfo = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(employeeDailyAttendance.Employee.EmployeeCardId);
                model.HourKey = employeeDailyAttendance.TransactionDateTime.ToString("hh").Length < 2 ? "0" + employeeDailyAttendance.TransactionDateTime.ToString("hh") : employeeDailyAttendance.TransactionDateTime.ToString("hh");
                model.MinuteKey = Convert.ToString(employeeDailyAttendance.TransactionDateTime.Minute).Length < 2 ? "0" + Convert.ToString(employeeDailyAttendance.TransactionDateTime.Minute) : Convert.ToString(employeeDailyAttendance.TransactionDateTime.Minute);
                model.PeriodKey = Convert.ToString(employeeDailyAttendance.TransactionDateTime.ToString("tt"));
                model.Date = employeeDailyAttendance.TransactionDateTime;
                model.Remarks = employeeDailyAttendance.Remarks;
                model.EmployeeCardId = employeeDailyAttendance.Employee.EmployeeCardId;              
            }

            return View(model);
        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() {EmployeeCardId = model.EmployeeCardId});
            if (!checkEmployeeCardId)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }

            var employeeDetails = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            if (employeeDetails == null)
            {
                return ErrorResult("Invalid Employee Id or Access Denied!");
            }

            return Json(new {EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true}, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "dailyattendance-2,dailyattendance-3")]
        public ActionResult Save(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var dailyAttendance = new EmployeeDailyAttendance()
                {
                    Id = model.Id,
                    EmployeeCardId = model.EmployeeCardId,
                    EmployeeId = model.EmployeeId,
                    TransactionDateTime = model.CustomDateTime,
                    IsFromMachine = model.IsFromMachine,
                    Remarks = model.Remarks,
                    IsActive = true
                };

                var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() {EmployeeCardId = model.EmployeeCardId});
                if (!checkEmployeeCardId)
                {
                    return ErrorResult("Invalid Id or Access denied!");
                }

                saveIndex = model.Id > 0 ? EmployeeDailyAttendanceManager.EditeEmployeeDailyAttendance(dailyAttendance) : EmployeeDailyAttendanceManager.SaveEmployeeDailyAttendance(dailyAttendance);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult EditBulkAttendance(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
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
                model.FromDate = DateTime.Now;
                model.ToDate = DateTime.Now;

                if (model.IsBulkAttendanceSearch)
                {
                    model.IsBulkAttendanceSearch = false;
                    return View(model);
                }
                var startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }
                var totalRecords = 0;
                model.Employees = EmployeeDailyAttendanceManager.GetEmployees(startPage, _pageSize, out totalRecords, model, model.SearchFieldModel);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "dailyattendance-2,dailyattendance-3")]
        public ActionResult SaveBulkAttendance(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            var saveIndex = 0;
            try
            {
                if (model.FromDate.HasValue && model.ToDate.HasValue && (model.ToDate.Value - model.FromDate.Value).Days >= 0)
                {
                    var selectedEmployeeDailyAttendances = model.GetSelectedEmployeeDailyAttendances();

                    if (model.EmployeeGuidIdList.Count > 0)
                    {
                        saveIndex = EmployeeDailyAttendanceManager.SaveEmployeeDailyAttendances(selectedEmployeeDailyAttendances);
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
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "dailyattendance-3")]
        public ActionResult Delete(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = EmployeeDailyAttendanceManager.DeleteEmployeeDailyAttendance(model.Id);
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
            return Json(new {Success = true, Branches = branches}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllBranchUnitByBranchId(int branchId)
        {
            var brancheUnits = BranchUnitManager.GetAllPermittedBranchUnitsByBranchId(branchId);
            return Json(new {Success = true, BrancheUnits = brancheUnits}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            return Json(new {Success = true, BranchUnitDepartments = branchUnitDepartments}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentSectionAndLineByBranchUnitDepartmentId(int branchUnitDepartmentId)
        {
            var departmentSections = DepartmentSectionManager.GetDepartmentSectionByBranchUnitDepartmentId(branchUnitDepartmentId);
            var departmentLines = DepartmentLineManager.GetDepartmentLineByBranchUnitDepartmentId(branchUnitDepartmentId);
            return Json(new {Success = true, DepartmentSections = departmentSections, DepartmentLines = departmentLines,}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranchUnitDepartmentAndWorkShiftByBranchUnitId(int branchUnitId)
        {
            var branchUnitDepartments = BranchUnitDepartmentManager.GetAllPermittedBranchUnitDepartmentsByBranchUnitId(branchUnitId);
            var branchUnitWorkShifts = BranchUnitWorkShiftManager.GetBranchUnitWorkShiftByBrancUnitId(branchUnitId);
            return Json(new {Success = true, BranchUnitDepartments = branchUnitDepartments, DataSources = branchUnitWorkShifts}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexDailyAttendance(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            ModelState.Clear();

            try
            {

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult ProcessDailyAttendance(Models.ViewModels.EmployeeDailyAttendanceViewModel model)
        {
            model.SearchFieldModel.EffectiveFromDate = model.FromDate;
            model.SearchFieldModel.ProcessType = "OT";
            int result = EmployeeDailyAttendanceManager.ProcessEmployeeInOut(model.SearchFieldModel);


            return null;
        }
    }
}