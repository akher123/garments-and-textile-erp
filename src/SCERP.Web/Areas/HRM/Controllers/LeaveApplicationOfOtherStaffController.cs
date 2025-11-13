using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;
using System.IO;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LeaveApplicationOfOtherStaffController : BaseHrmController
    {

        private readonly int _pageSize = AppConfig.PageSize;

        [AjaxAuthorize(Roles = "otherstaff-1,otherstaff-2,otherstaff-3")]
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

            if (model.IsSearch)
            {
                model.IsSearch = false;
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.SearchByEmployeeCardId))
            {
                var checkEmployeeCardId = false;

                checkEmployeeCardId = EmployeeManager.CheckExistingEmployeeCardNumber(new Employee() { EmployeeCardId = model.SearchByEmployeeCardId });
                if (checkEmployeeCardId)
                    employeeLeave.EmployeeId = EmployeeManager.GetEmployeeByCardId(model.SearchByEmployeeCardId).EmployeeId;

                if (!checkEmployeeCardId)
                    return ErrorResult("Invalid EmployeeId, Please Insert a valid user.");
            }


            var startPage = 0;
            if (model.page.HasValue && model.page.Value > 0)
            {
                startPage = model.page.Value - 1;
            }

            int totalRecords = 0;
            model.EmployeeLeaves = EmployeeLeaveManager.GetAllAppliedEmployeeLeavesByPaging(startPage, _pageSize, employeeLeave, out totalRecords) ?? new List<EmployeeLeave>();
            model.TotalRecords = totalRecords;
            return View(model);
        }

        [AjaxAuthorize(Roles = "otherstaff-2,otherstaff-3")]
        public ActionResult Edit(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();
            try
            {
                var recommendationEmployeeList = EmployeeLeaveManager.GetAuthorizedPersons((int)SCERP.Common.ProcessKeyEnum.HRM_Leave, Convert.ToInt32(SCERP.Common.AuthorizationType.LeaveRecommendation));


                var approvalEmployeeList = EmployeeLeaveManager.GetAuthorizedPersons((int)ProcessKeyEnum.HRM_Leave, Convert.ToInt32(SCERP.Common.AuthorizationType.LeaveApproval));

                if (model.Id > 0)
                {
                    var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id);
                    model.EmployeeId = employeeLeave.EmployeeId;
                    model.EmployeeCardId = employeeLeave.EmployeeCardId;
                    if (employeeLeave.AppliedFromDate != null)
                    {
                        model.Year = employeeLeave.AppliedFromDate.Value.Year;
                    }
                    model.LeaveTypeId = employeeLeave.LeaveTypeId;
                    model.SubmitDate = employeeLeave.SubmitDate;
                    model.AppliedFromDate = employeeLeave.AppliedFromDate;
                    model.AppliedToDate = employeeLeave.AppliedToDate;
                    model.AppliedTotalDays = employeeLeave.AppliedTotalDays;
                    model.LeavePurpose = employeeLeave.LeavePurpose;
                    model.EmergencyPhoneNo = employeeLeave.EmergencyPhoneNo;
                    model.AddressDuringLeave = employeeLeave.AddressDuringLeave;

                    model.RecommendedFromDate = employeeLeave.RecommendedFromDate;
                    model.RecommendedToDate = employeeLeave.RecommendedToDate;
                    model.RecommendedTotalDays = employeeLeave.RecommendedTotalDays;
                    model.RecommendationStatus = employeeLeave.RecommendationStatus;
                    model.RecommendationStatusDate = employeeLeave.RecommendationStatusDate;
                    model.RecommendationPerson = employeeLeave.RecommendationPerson;
                    model.RecommendationComment = employeeLeave.RecommendationComment;

                    model.ApprovedFromDate = employeeLeave.ApprovedFromDate;
                    model.ApprovedToDate = employeeLeave.ApprovedToDate;
                    model.ApprovedTotalDays = employeeLeave.ApprovedTotalDays;
                    model.ApprovalStatus = employeeLeave.ApprovalStatus;
                    model.ApprovalStatusDate = employeeLeave.ApprovalStatusDate;
                    model.ApprovalPerson = employeeLeave.ApprovalPerson;
                    model.ApprovalComment = employeeLeave.ApprovalComment;

                    model.ConsumedFromDate = employeeLeave.ConsumedFromDate;
                    model.ConsumedToDate = employeeLeave.ConsumedToDate;
                    model.ConsumedTotalDays = employeeLeave.ConsumedTotalDays;

                    model.JoinedBeforeDays = employeeLeave.JoinedBeforeDays;
                    model.ResumeDate = employeeLeave.ResumeDate;

                }
                else
                {
                    model.AppliedFromDate = DateTime.Now;
                    model.AppliedToDate = DateTime.Now;
                    model.Year = DateTime.Now.Year;
                }

                model.RecommendationPersons = recommendationEmployeeList;
                model.ApprovalPersons = approvalEmployeeList;
                model.LeaveTypes = EmployeeLeaveManager.GetAllLeaveType().ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }

        [AjaxAuthorize(Roles = "otherstaff-2,otherstaff-3")]
        public ActionResult Save(EmployeeLeaveViewModel model)
        {
            var saveIndex = 0;

            try
            {
                var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id) ?? new EmployeeLeave();
                var employee = EmployeeManager.GetEmployeeByCardId(model.EmployeeCardId);
                employeeLeave.EmployeeId = employee.EmployeeId;
                employeeLeave.EmployeeCardId = model.EmployeeCardId;
                employeeLeave.LeaveTypeId = model.LeaveTypeId;
                employeeLeave.SubmitDate = model.SubmitDate;
                employeeLeave.AppliedFromDate = model.AppliedFromDate;
                employeeLeave.AppliedToDate = model.AppliedToDate;
                employeeLeave.AppliedTotalDays = model.AppliedTotalDays;
                employeeLeave.LeavePurpose = model.LeavePurpose;
                employeeLeave.EmergencyPhoneNo = model.EmergencyPhoneNo;
                employeeLeave.AddressDuringLeave = model.AddressDuringLeave;


                employeeLeave.RecommendedFromDate = model.RecommendedFromDate;
                employeeLeave.RecommendedToDate = model.RecommendedToDate;
                employeeLeave.RecommendedTotalDays = model.RecommendedTotalDays;
                employeeLeave.RecommendationStatus = model.RecommendationStatus;
                employeeLeave.RecommendationStatusDate = model.RecommendationStatusDate;
                employeeLeave.RecommendationPerson = model.RecommendationPerson;
                employeeLeave.RecommendationComment = model.RecommendationComment;

                employeeLeave.ApprovedFromDate = model.ApprovedFromDate;
                employeeLeave.ApprovedToDate = model.ApprovedToDate;
                employeeLeave.ApprovedTotalDays = model.ApprovedTotalDays;
                employeeLeave.ApprovalStatus = model.ApprovalStatus;
                employeeLeave.ApprovalStatusDate = model.ApprovalStatusDate;
                employeeLeave.ApprovalPerson = model.ApprovalPerson;
                employeeLeave.ApprovalComment = model.ApprovalComment;

                employeeLeave.ConsumedFromDate = model.ConsumedFromDate;
                employeeLeave.ConsumedToDate = model.ConsumedToDate;
                employeeLeave.ConsumedTotalDays = model.ConsumedTotalDays;

                employeeLeave.JoinedBeforeDays = model.JoinedBeforeDays;

                //bool leavePermitStatus = EmployeeLeaveManager.CheckLeaveValidity(employeeLeave.EmployeeId, employeeLeave.EmployeeCardId, model.Year, employeeLeave.LeaveTypeId, employeeLeave.AppliedTotalDays);

                //switch (leavePermitStatus)
                //{
                //    case true:
                //        {
                //            saveIndex = (model.Id > 0) ? EmployeeLeaveManager.EditEmployeeLeave(employeeLeave) : EmployeeLeaveManager.SaveEmployeeLeave(employeeLeave);
                //        }
                //        break;
                //    default:
                //        return ErrorResult("You do not have enough available days. Please apply for another leave type!");
                //}

                if (model.Id > 0)
                {
                    var employeeSpecificLeave = EmployeeLeaveManager.GetEmployeeLeaveById(model.Id);
                    var deleted = EmployeeLeaveManager.DeleteEmployeeLeave(employeeSpecificLeave);

                    if (deleted <= 0)
                    {
                        return ErrorResult("Failed to edit leave!");
                    }
                }

                bool employeeLeaveExistence = EmployeeLeaveManager.CheckEmployeeLeaveExistence(employeeLeave);

                if (employeeLeaveExistence)
                {
                    return ErrorResult("Leave day(s) already exists!");
                }

                bool leavePermitStatus = EmployeeLeaveManager.CheckLeaveValidity(employeeLeave.EmployeeId, employeeLeave.EmployeeCardId, model.Year, employeeLeave.LeaveTypeId, employeeLeave.AppliedTotalDays);
                switch (leavePermitStatus)
                {
                    case true:
                        {
                            saveIndex = (model.Id > 0) ? EmployeeLeaveManager.EditEmployeeLeave(employeeLeave) : EmployeeLeaveManager.SaveEmployeeLeave(employeeLeave);
                        }
                        break;
                    default:
                        return ErrorResult("You do not have enough available days. Please apply for another leave type!");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        [AjaxAuthorize(Roles = "otherstaff-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(id) ?? new EmployeeLeave();
            deleted = EmployeeLeaveManager.DeleteEmployeeLeave(employeeLeave);
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        [AjaxAuthorize(Roles = "otherstaff-1,otherstaff-2,otherstaff-3")]
        public ActionResult GetEmployeeData(string employeeCardId, int year)
        {
            const bool value = true;
            var empData = EmployeeLeaveManager.GetEmployeeData(employeeCardId);
            //var empLeaveData = EmployeeLeaveManager.GetEmployeeLeaveData(employeeCardId);
            var empLeaveData = EmployeeLeaveManager.GetEmployeeLeaveData(employeeCardId, year);
            return empData == null ? Json(new { Sucess = "false" }) : Json(new { data = empData, leavedata = RenderViewToString("_LeaveRemain", empLeaveData), Success = value });
        }
    }
}