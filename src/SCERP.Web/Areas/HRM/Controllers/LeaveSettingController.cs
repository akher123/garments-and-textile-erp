using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

using System.Web.Helpers;
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
    public class LeaveSettingController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "leave-1,leave-2,leave-3")]
        public ActionResult Index(LeaveSettingViewModel model)
        {
            try
            {
                ModelState.Clear();
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.EmployeeTypes = LeaveSettingManager.GetAllEmployeeType();
                model.LeaveTypes = LeaveSettingManager.GetAllLeaveType();

                //if (model.IsSearch)
                //{
                //    model.IsSearch = false;
                //    return View(model);
                //}

                model.BranchUnits = UnitManager.GetAllUnitsByCompanyId(model.SearchFieldModel.SearchByCompanyId);

                var totalRecords = 0;
                var startPage = 0;

                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                model.LeaveSettings = LeaveSettingManager.GetAllLeaveSettings(startPage, _pageSize, model, model.SearchFieldModel, out totalRecords);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "leave-2,leave-3")]
        public ActionResult Edit(LeaveSettingViewModel model)
        {
            ModelState.Clear();
            try
            {
                model.Companies = CompanyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
                model.EmployeeTypes = LeaveSettingManager.GetAllEmployeeType();
                model.LeaveTypes = LeaveSettingManager.GetAllLeaveType();

                if (model.Id > 0)
                {
                    var leaveSetting = LeaveSettingManager.GetLeaveSettingById(model.Id);

                    model.EmployeeTypeId = leaveSetting.EmployeeTypeId;
                    model.LeaveTypeId = leaveSetting.LeaveTypeId;
                    model.NoOfDays = leaveSetting.NoOfDays;
                    model.MaximumAtATime = leaveSetting.MaximumAtATime;

                    model.CompanyId = leaveSetting.BranchUnit.Branch.CompanyId;
                    model.BranchUnits = UnitManager.GetAllUnitsByCompanyId(model.CompanyId);
                    model.BranchUnitId = leaveSetting.BranchUnitId;

                    model.CreatedBy = leaveSetting.CreatedBy;
                    model.EditedBy = leaveSetting.EditedBy;
                    model.CreatedDate = leaveSetting.CreatedDate;
                    model.EditedDate = leaveSetting.EditedDate;
                    model.IsActive = leaveSetting.IsActive;
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "leave-2,leave-3")]
        public ActionResult Save(LeaveSettingViewModel model)
        {
            var saveIndex = 0;
            try
            {
                var isExist = LeaveSettingManager.IsExistLeaveSetting(model);
                switch (isExist)
                {
                    case false:
                        {
                            var leaveSetting = LeaveSettingManager.GetLeaveSettingById(model.Id) ?? new LeaveSetting();
                            leaveSetting.BranchUnitId = model.BranchUnitId;
                            leaveSetting.EmployeeTypeId = model.EmployeeTypeId;
                            leaveSetting.LeaveTypeId = model.LeaveTypeId;
                            leaveSetting.NoOfDays = model.NoOfDays;
                            leaveSetting.MaximumAtATime = model.MaximumAtATime;
                            saveIndex = LeaveSettingManager.SaveLeaveSetting(leaveSetting);

                        }
                        break;
                    default:
                        return ErrorResult("Leave setup already exist!");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data!");
        }

        [AjaxAuthorize(Roles = "leave-3")]
        public ActionResult Delete(int id)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = LeaveSettingManager.DeleteLeaveSetting(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleteIndex > 0) ? Reload() : ErrorResult("Failed to delete data!");

        }

        public ActionResult GetAllUnitsByCompanyId(int companyId)
        {
            var branchUnits = UnitManager.GetAllUnitsByCompanyId(companyId);
            return Json(new { Success = true, BranchUnits = branchUnits }, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize(Roles = "leave-1,leave-2,leave-3")]
        public void GetExcel(LeaveSettingViewModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                model.SearchFieldModel = searchFieldModel;
                model.LeaveSettings = LeaveSettingManager.GetAllLeaveSettingsBySearchKey(model, searchFieldModel);

                const string fileName = "LeaveSetting";
                var boundFields = new List<BoundField>
            {
               new BoundField(){HeaderText = @"Company",DataField = "BranchUnit.Branch.Company.Name"},
               new BoundField(){HeaderText = @"Unit",DataField = "BranchUnit.Unit.Name"},
               new BoundField(){HeaderText = @"Employee Type",DataField = "EmployeeType.Title"},
               new BoundField(){HeaderText = @"Leave Type",DataField = "LeaveType.Title"},
               new BoundField(){HeaderText = @"No Of Days",DataField = "NoOfDays"},
               new BoundField(){HeaderText = @"Maximum at a time",DataField = "MaximumAtATime"},

            };
                ReportConverter.CustomGridView(boundFields, model.LeaveSettings, fileName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

        }

        [AjaxAuthorize(Roles = "leave-1,leave-2,leave-3")]
        public ActionResult Print(LeaveSettingViewModel model, SearchFieldModel searchFieldModel)
        {
            try
            {
                model.SearchFieldModel = searchFieldModel;
                model.LeaveSettings = LeaveSettingManager.GetAllLeaveSettingsBySearchKey(model, searchFieldModel);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return PartialView("_LeaveSettingPrintPreview", model);
        }

    }
}