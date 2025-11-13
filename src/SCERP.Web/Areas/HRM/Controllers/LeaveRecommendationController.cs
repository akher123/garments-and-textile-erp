using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LeaveRecommendationController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        [AjaxAuthorize(Roles = "leaverecommendation-1,leaverecommendation-2,leaverecommendation-3")]
        public ActionResult Index(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();
            EmployeeLeave employeeLeave = model;

            employeeLeave.AppliedFromDate = model.SearchByAppliedDateFrom;
            employeeLeave.AppliedToDate = model.SearchByAppliedDateTo;
            employeeLeave.RecommendedFromDate = model.SearchByRecommendedDateFrom;
            employeeLeave.RecommendedToDate = model.SearchByRecommendedDateTo;
            employeeLeave.RecommendationPerson = _employeeGuidId;

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.SearchByEmployeeCardId))
                employeeLeave.EmployeeId = Guid.Empty;
            else
            {
                var checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.SearchByEmployeeCardId });

                if (!checkEmployeeCardId)
                    return ErrorResult("Invalid Employee Id, Please Insert a valid employee.");

                employeeLeave.EmployeeId = EmployeeManager.GetEmployeeByCardId(model.SearchByEmployeeCardId).EmployeeId;
            }

            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            var totalRecords = 0;
            model.EmployeeLeaves = EmployeeLeaveManager.GetAllAppliedEmployeeLeavesByPaging(startPage, _pageSize, employeeLeave, out totalRecords) ?? new List<EmployeeLeave>();
            model.TotalRecords = totalRecords;

            return View(model);
        }


        [AjaxAuthorize(Roles = "leaverecommendation-2,leaverecommendation-3")]
        public ActionResult Edit(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();

            try
            {
                var recommendationStatusList = from LeaveRecommendation leaveStatus in Enum.GetValues(typeof(LeaveRecommendation))
                                           select new { Id = (int)leaveStatus, Name = leaveStatus.ToString() };

                if (model.Id != 0)
                {
                    var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id);

                    model.EmployeeId = employeeLeave.EmployeeId;
                    model.EmployeeCardId = EmployeeManager.GetEmployeeById(employeeLeave.EmployeeId).EmployeeCardId;
                    model.LeaveTypeId = employeeLeave.LeaveTypeId;

                    model.RecommendedFromDate = employeeLeave.RecommendedFromDate ?? employeeLeave.AppliedFromDate;
                    model.RecommendedToDate = employeeLeave.RecommendedToDate ?? employeeLeave.AppliedToDate;
                    model.RecommendedTotalDays = employeeLeave.RecommendedTotalDays ?? employeeLeave.AppliedTotalDays;

                    model.RecommendationComment = employeeLeave.RecommendationComment;
                    model.AppliedFromDate = employeeLeave.AppliedFromDate;
                    model.AppliedToDate = employeeLeave.AppliedToDate;
                    model.AppliedTotalDays = employeeLeave.AppliedTotalDays;
                    model.LeavePurpose = employeeLeave.LeavePurpose;
                    model.RecommendationPerson = employeeLeave.RecommendationPerson;
                    model.AddressDuringLeave = employeeLeave.AddressDuringLeave;
                    model.EmergencyPhoneNo = employeeLeave.EmergencyPhoneNo;
                    model.LeaveTypeId = employeeLeave.LeaveTypeId;
                    model.RecommendationStatus = employeeLeave.RecommendationStatus;
                    model.LeaveRecommendationStatus = recommendationStatusList;
                }
                model.LeaveTypes = EmployeeLeaveManager.GetAllLeaveType().ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "leaverecommendation-2,leaverecommendation-3")]
        public ActionResult Save(EmployeeLeaveViewModel model)
        {
            var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id) ?? new EmployeeLeave();

            employeeLeave.EmployeeId = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId).EmployeeId;

            employeeLeave.RecommendedFromDate = model.RecommendedFromDate;
            employeeLeave.RecommendedToDate = model.RecommendedToDate;
            employeeLeave.RecommendedTotalDays = model.RecommendedTotalDays;

            employeeLeave.RecommendationStatus = model.RecommendationStatus;

            employeeLeave.RecommendationComment = model.RecommendationComment;
            employeeLeave.RecommendationStatusDate = DateTime.Now;

            var saveIndex = (model.Id > 0) ? EmployeeLeaveManager.EditEmployeeLeave(employeeLeave) : EmployeeLeaveManager.SaveEmployeeLeave(employeeLeave);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

    }
}
