using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text.pdf.qrcode;
using SCERP.Common;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Model;
using SCERP.Web.Controllers;
using System.IO;

namespace SCERP.Web.Areas.HRM.Controllers
{
    public class LeaveApplicationOfPaperBasedController : BaseHrmController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly Guid? _employeeGuidId = PortalContext.CurrentUser.UserId;

        [AjaxAuthorize(Roles = "paperbased-1,paperbased-2,paperbased-3")]
        public ActionResult Index(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();
            EmployeeLeave employeeLeave = model;

            employeeLeave.AppliedFromDate = model.SearchByAppliedDateFrom;
            employeeLeave.AppliedToDate = model.SearchByAppliedDateTo;

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

        public bool IsGuid(Guid id)
        {
            if (id == Guid.Empty)
                return true;
            else
                return false;
        }

        [AjaxAuthorize(Roles = "paperbased-2,paperbased-3")]
        public ActionResult Edit(EmployeeLeaveViewModel model)
        {
            ModelState.Clear();

            try
            {

                var recommendationEmployeeList = EmployeeLeaveManager.GetAuthorizedPersons((int)ProcessKeyEnum.HRM_Leave, Convert.ToInt32(SCERP.Common.AuthorizationType.LeaveRecommendation));


                var approvalStatusList = from LeaveStatusHR leaveStatusWorker in Enum.GetValues(typeof(LeaveStatusHR))
                                         where Convert.ToInt32(leaveStatusWorker) == Convert.ToInt32(LeaveStatusHR.Approved)
                                         select new { Id = (int)leaveStatusWorker, Name = leaveStatusWorker.ToString() };

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

                    model.RecommendedFromDate = employeeLeave.AppliedFromDate;
                    model.RecommendedToDate = employeeLeave.AppliedToDate;
                    model.RecommendedTotalDays = employeeLeave.AppliedTotalDays;
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
                model.LeaveApprovalStatus = approvalStatusList;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }


        [AjaxAuthorize(Roles = "paperbased-2,paperbased-3")]
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

                employeeLeave.RecommendedFromDate = model.AppliedFromDate;
                employeeLeave.RecommendedToDate = model.AppliedToDate;
                employeeLeave.RecommendedTotalDays = model.AppliedTotalDays;
                employeeLeave.RecommendationStatus = Convert.ToInt32(LeaveRecommendation.Recommended);
                employeeLeave.RecommendationStatusDate = model.SubmitDate;
                employeeLeave.RecommendationPerson = model.RecommendationPerson;
                employeeLeave.RecommendationComment = "Recommendation on Paper";

                employeeLeave.ApprovedFromDate = model.AppliedFromDate;
                employeeLeave.ApprovedToDate = model.AppliedToDate;
                employeeLeave.ApprovedTotalDays = model.AppliedTotalDays;
                employeeLeave.ApprovalStatus = model.ApprovalStatus;
                employeeLeave.ApprovalStatusDate = model.SubmitDate;
                employeeLeave.ApprovalPerson = model.ApprovalPerson;
                employeeLeave.ApprovalComment = "Approved on Paper";

                employeeLeave.ConsumedFromDate = model.AppliedFromDate;
                employeeLeave.ConsumedToDate = model.AppliedToDate;
                employeeLeave.ConsumedTotalDays = model.AppliedTotalDays;

                employeeLeave.JoinedBeforeDays = 0;

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

            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save leave!");
        }


        [AjaxAuthorize(Roles = "paperbased-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            try
            {
                var employeeLeave = EmployeeLeaveManager.GetEmployeeLeaveById(id) ?? new EmployeeLeave();
                deleted = EmployeeLeaveManager.DeleteEmployeeLeave(employeeLeave);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete leave!");
        }

        [AjaxAuthorize(Roles = "paperbased-1,paperbased-2,paperbased-3")]
        public ActionResult GetEmployeeData(string employeeCardId, int year)
        {
            const bool value = true;
            var empData = EmployeeLeaveManager.GetEmployeeData(employeeCardId);

            //var empLeaveData = EmployeeLeaveManager.GetEmployeeLeaveData(employeeCardId);
            var empLeaveData = EmployeeLeaveManager.GetEmployeeLeaveData(employeeCardId, year);

            if (empLeaveData == null)
                return ErrorResult("Employee doesn't have enough information!");

            return empData == null ? Json(new { Sucess = "false" }) : Json(new { data = empData, leavedata = RenderViewToString("_LeaveRemain", empLeaveData), Success = value });
        }

        public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
