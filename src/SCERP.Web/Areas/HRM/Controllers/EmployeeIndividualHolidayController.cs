using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using System.Collections.Generic;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class EmployeeIndividualHolidayController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        public ActionResult Index(EmployeeLineInfoViewModel model)
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

                model.EmployeeCompanyInfoModels = EmployeeCompanyInfoManager.GetEmployeeIndividualHoliday(startPage, _pageSize, model, model.SearchFieldModel);
                model.TotalRecords = model.EmployeeCompanyInfoModels[0].TotalRows;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult AssignEmployeeLineIndex(EmployeeLineInfoViewModel model)
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

                ViewBag.LeaveType = new SelectList(new[] {new {Id = "Weekend", Value = "Weekend"}, new {Id = "Holiday", Value = "Holiday"}}, "Id", "Value");


                //if (model.IsSearch)
                //{
                //    model.IsSearch = true;
                //    return View(model);
                //}

                model.EmployeeCompanyInfoModels = EmployeeCompanyInfoManager.GetEmployeesForIndividualHoliday(model, model.SearchFieldModel);

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        public ActionResult AssignEmployeeLine(EmployeeLineInfoViewModel model)
        {
            try
            {
                var saveIndex = 0;
                model.SearchFieldModel.EmployeeIdList = new List<Guid>();
                model.SearchFieldModel.EmployeeIdList = model.EmployeeIdList;

                string leaveType = model.LeaveType;

                if (model.SearchFieldModel.EmployeeIdList.Count > 0 && leaveType == "Holiday")
                    saveIndex = EmployeeCompanyInfoManager.AssignIndividualHoliday(model.SearchFieldModel, model);

                else if(model.SearchFieldModel.EmployeeIdList.Count > 0 && leaveType == "Weekend")
                    saveIndex = EmployeeCompanyInfoManager.AssignIndividualWeekend(model.SearchFieldModel, model);

                else
                    return ErrorResult("Please select any one for processing");

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
    }
}