using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCERP.Common;
using SCERP.Common.ReportHelper;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class OutStationDutyController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "outstationduty-1,outstationduty-2,outstationduty-3")]
        public ActionResult Index(OutStationDutyViewModel model)
        {

            try
            {
                ModelState.Clear();
                model.Companies = CompanyManager.GetAllPermittedCompanies();
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
                model.VOutStationDutyDetails = OutStationDutyManager.GetAllOutStationDutyDetail(startPage, _pageSize, model, model.SearchFieldModel, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        [AjaxAuthorize(Roles = "outstationduty-2,outstationduty-3")]
        public ActionResult Edit(OutStationDutyViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.OutStationDutyId > 0)
                {
                    var outStationDuty = OutStationDutyManager.GetOutStationDutyById(model.OutStationDutyId);
                    model.OutStationDutyId = outStationDuty.OutStationDutyId;
                    model.Purpose = outStationDuty.Purpose;
                    model.Location = outStationDuty.Location;
                    model.DutyDate = outStationDuty.DutyDate;
                    model.EmployeeCardId = outStationDuty.Employee.EmployeeCardId;
                    model.VEmployeeCompanyInfoDetail = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(outStationDuty.Employee.EmployeeCardId);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "outstationduty-2,outstationduty-3")]
        public JsonResult Save(OutStationDutyViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
                if (!checkEmployeeCardId)
                {
                    return ErrorResult("Invalid Id or Access denied!");
                }
                else
                {
                    var osd = new OutStationDuty()
                    {
                        OutStationDutyId = model.OutStationDutyId,
                        Purpose = model.Purpose,
                        Location = model.Location,
                        DutyDate = model.DutyDate,
                        EmployeeId = model.EmployeeId,
                    };
                    saveIndex = model.OutStationDutyId > 0 ? OutStationDutyManager.EditOutStationDuty(osd) : OutStationDutyManager.SaveOutStationDuty(osd);
                }


            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0 ? Reload() : ErrorResult("Failed to save data ");
        }

        [AjaxAuthorize(Roles = "outstationduty-3")]
        public ActionResult Delete(OutStationDutyViewModel mode)
        {
            var deleted = 0;
            try
            {

                deleted = OutStationDutyManager.DeleteOutstationDutyById(mode.OutStationDutyId);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data!");

        }

        public ActionResult GetEmployeeDetailByEmployeeCadId(OutStationDutyViewModel model)
        {

            var checkEmployeeCardId =
                EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.EmployeeCardId });
            if (!checkEmployeeCardId)
            {
                return Json(new { Message = "Invalid ID or Access Denied!", ValidStatus = false }, JsonRequestBehavior.AllowGet);
            }
            var employeeDetails = EmployeeDailyAttendanceManager.GetEmployeeByEmployeeCardId(model.EmployeeCardId);

            return Json(new { EmployeeDetailView = RenderViewToString("_EmployeeDetails", employeeDetails), Success = true }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "outstationduty-1,outstationduty-2,outstationduty-3")]
        public void GetExcel(SearchFieldModel searchField)
        {
            var model = new OutStationDutyViewModel();
            try
            {
                model.VOutStationDutyDetails = OutStationDutyManager.GetOutStationDutyBySearchKey(searchField);
                const string fileName = "OutStationDuty";
                var boundFields = new List<BoundField>
                 {
                   new BoundField(){HeaderText = @"Employee Name",DataField = "EmployeeName"},
                   new BoundField(){HeaderText = @"Employee ID",DataField = "EmployeeCardId"},
                   new BoundField(){HeaderText = @"DutyDate",DataField = "DutyDate",DataFormatString = "{0:dd/MM/yyyy}"},
                   new BoundField(){HeaderText = @"Location",DataField = "Location"},
                   new BoundField(){HeaderText = @"Purpose",DataField = "Purpose"},
                   new BoundField(){HeaderText = @"Company",DataField = "CompanyName"},
                   new BoundField(){HeaderText = @"Branch",DataField = "BranchName"}, 
                   new BoundField(){HeaderText = @"Unit",DataField = "UnitName"},
                   new BoundField(){HeaderText = @"DepartmentName",DataField = "DepartmentName"},
                   new BoundField(){HeaderText = @"Section",DataField = "SectionName"}, 
                   new BoundField(){HeaderText = @"Line",DataField = "LineName"},
                   
                 };
                ReportConverter.CustomGridView(boundFields, model.VOutStationDutyDetails, fileName);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "outstationduty-1,outstationduty-2,outstationduty-3")]
        public PartialViewResult Print(SearchFieldModel searchField)
        {
            var model = new OutStationDutyViewModel();
            try
            {

                model.VOutStationDutyDetails = OutStationDutyManager.GetOutStationDutyBySearchKey(searchField);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return PartialView("_OutStationDutyReport", model);
        }

    }
}