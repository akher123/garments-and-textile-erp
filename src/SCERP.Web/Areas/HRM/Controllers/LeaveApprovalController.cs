using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Models.ViewModels;

using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LeaveApprovalController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;

        private Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        [AjaxAuthorize(Roles = "leaveapproval-1,leaveapproval-2,leaveapproval-3")]
        public ActionResult Index(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();
            EmployeeLeave employeeLeave = model;

            employeeLeave.AppliedFromDate = model.SearchByAppliedDateFrom;
            employeeLeave.AppliedToDate = model.SearchByAppliedDateTo;
            employeeLeave.RecommendedFromDate = model.SearchByRecommendedDateFrom;
            employeeLeave.RecommendedToDate = model.SearchByRecommendedDateTo;
            employeeLeave.ApprovedFromDate = model.SearchByApprovalDateFrom;
            employeeLeave.ApprovedToDate = model.SearchByApprovalDateTo;

            employeeLeave.ApprovalPerson = _employeeGuidId;

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.SearchByEmployeeCardId))
                employeeLeave.EmployeeId = Guid.Empty;
            else
            {
                bool checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.SearchByEmployeeCardId });

                if (!checkEmployeeCardId)
                    return ErrorResult("Invalid EmployeeId, Please Insert a valid user.");
                else
                    employeeLeave.EmployeeId = EmployeeManager.GetEmployeeByCardId(model.SearchByEmployeeCardId).EmployeeId;
            }


            bool checkApprovedPerson = EmployeeLeaveManager.CheckAuthorizedPerson(_employeeGuidId, (int)SCERP.Common.ProcessKeyEnum.HRM_Leave, (int)SCERP.Common.AuthorizationType.LeaveApproval);

            if (!checkApprovedPerson)
            {
                model.EmployeeLeaves = new List<EmployeeLeave>();
                return View(model);
            }

            
            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.EmployeeLeaves = EmployeeLeaveManager.GetAllRecommendedEmployeeLeavesByPaging(startPage, _pageSize, out totalRecords, employeeLeave) ?? new List<EmployeeLeave>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

       [AjaxAuthorize(Roles = "leaveapproval-2,leaveapproval-3")]
        public ActionResult Edit(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();

            try
            {
                var recommendationStatusList = from LeaveRecommendation leaveStatus in Enum.GetValues(typeof(LeaveRecommendation))
                                           select new { Id = (int)leaveStatus, Name = leaveStatus.ToString() };

                var approvalStatusList = from LeaveStatusHR leaveStatus in Enum.GetValues(typeof(LeaveStatusHR))
                                         select new { Id = (int)leaveStatus, Name = leaveStatus.ToString() };

                if (model.Id != 0)
                {
                    var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id);
                    model.EmployeeId = employeeLeave.EmployeeId;
                    model.EmployeeCardId = EmployeeManager.GetEmployeeById(employeeLeave.EmployeeId).EmployeeCardId;
                    model.LeaveTypeId = employeeLeave.LeaveTypeId;
                    model.SubmitDate = employeeLeave.SubmitDate;

                    model.AppliedFromDate = employeeLeave.AppliedFromDate;
                    model.AppliedToDate = employeeLeave.AppliedToDate;
                    model.AppliedTotalDays = employeeLeave.AppliedTotalDays;
                    model.LeavePurpose = employeeLeave.LeavePurpose;
                    model.AddressDuringLeave = employeeLeave.AddressDuringLeave;
                    model.EmergencyPhoneNo = employeeLeave.EmergencyPhoneNo;


                    model.RecommendedFromDate = employeeLeave.RecommendedFromDate;
                    model.RecommendedToDate = employeeLeave.RecommendedToDate;
                    model.RecommendedTotalDays = employeeLeave.RecommendedTotalDays;
                    model.RecommendationStatus = employeeLeave.RecommendationStatus;
                    model.RecommendationStatusDate = employeeLeave.RecommendationStatusDate;
                    model.RecommendationComment = employeeLeave.RecommendationComment;
                    model.RecommendationPerson = employeeLeave.RecommendationPerson;

                    model.ApprovedFromDate = employeeLeave.ApprovedFromDate;
                    model.ApprovedToDate = employeeLeave.ApprovedToDate;
                    model.ApprovedTotalDays = employeeLeave.ApprovedTotalDays;
                    model.ApprovalStatus = employeeLeave.ApprovalStatus;
                    model.ApprovalStatusDate = employeeLeave.ApprovalStatusDate;
                    model.ApprovalComment = employeeLeave.ApprovalComment;
                    model.ApprovalPerson = employeeLeave.ApprovalPerson;

                    model.ConsumedFromDate = employeeLeave.ConsumedFromDate;
                    model.ConsumedToDate = employeeLeave.ConsumedToDate;
                    model.ConsumedTotalDays = employeeLeave.ConsumedTotalDays;

                    model.ResumeDate = employeeLeave.ResumeDate;
                    model.JoinedBeforeDays = employeeLeave.JoinedBeforeDays;
                   
                }

                model.LeaveStatusHr = approvalStatusList;
                model.LeaveRecommendationStatus = recommendationStatusList;
                model.LeaveTypes = EmployeeLeaveManager.GetAllLeaveType().ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "leaveapproval-2,leaveapproval-3")]
        public ActionResult Save(EmployeeLeaveViewModel model)
        {
            var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id) ?? new EmployeeLeave();
           
            employeeLeave.ApprovedFromDate = model.ApprovedFromDate;
            employeeLeave.ApprovedToDate = model.ApprovedToDate;
            employeeLeave.ApprovedTotalDays = model.ApprovedTotalDays;
            employeeLeave.ApprovalStatus = model.ApprovalStatus;
            employeeLeave.ApprovalStatusDate = model.ApprovalStatusDate;
            employeeLeave.ApprovalComment = model.ApprovalComment;
            employeeLeave.ApprovalPerson = model.ApprovalPerson;

            employeeLeave.ConsumedFromDate = model.ConsumedFromDate;
            employeeLeave.ConsumedToDate = model.ConsumedToDate;
            employeeLeave.ConsumedTotalDays = model.ConsumedTotalDays;

            employeeLeave.ResumeDate = model.ResumeDate;
            employeeLeave.JoinedBeforeDays = model.JoinedBeforeDays;
                   
            employeeLeave.ApprovalPerson = _employeeGuidId;
            var saveIndex = (model.Id > 0) ? EmployeeLeaveManager.EditEmployeeLeave(employeeLeave) : EmployeeLeaveManager.SaveEmployeeLeave(employeeLeave);
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }
    }
}