using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeLeaveViewModel : EmployeeLeave
    {
        public EmployeeLeaveViewModel()
        {
            EmployeeLeaves = new List<EmployeeLeave>();
            RecommendationPersons = new List<Employee>();
            ApprovalPersons = new List<Employee>();
            LeaveTypes = new List<LeaveType>();
            IsSearch = true;
        }


        public List<EmployeeLeave> EmployeeLeaves { get; set; }
        public List<EmployeeLeave> EmployeeLeavesHistory { get; set; }
        public string ApplicationFor { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string EmployeeCardId { get; set; }

        //[Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SearchByEmployeeCardId
        {
            get;
            set;
        }


        public Nullable<DateTime> SearchByAppliedDateFrom
        {
            get;
            set;
        }

        public Nullable<DateTime> SearchByAppliedDateTo
        {
            get;
            set;
        }

        public Nullable<DateTime> SearchByRecommendedDateFrom
        {
            get;
            set;
        }

        public Nullable<DateTime> SearchByRecommendedDateTo
        {
            get;
            set;
        }

        public Nullable<DateTime> SearchByApprovalDateFrom
        {
            get;
            set;
        }

        public Nullable<DateTime> SearchByApprovalDateTo
        {
            get;
            set;
        }

        public IEnumerable LeaveRecommendationStatus { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> LeaveStatusWorkerSelectListItem
        {
            get { return new SelectList(LeaveRecommendationStatus, "Id", "Name"); }
        }
       
        public List<Employee> RecommendationPersons { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RecommendationPersonSelectListItem
        {
            get { return new SelectList(RecommendationPersons, "EmployeeId", "Name"); }
        }

        public List<LeaveType> LeaveTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> LeaveTypeSelectListItem
        {
            get { return new SelectList(LeaveTypes, "Id", "Title"); }
        }
        public IEnumerable LeaveStatusHr { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ApprovalStatusSelectListItem
        {
            get { return new SelectList(LeaveStatusHr, "Id", "Name"); }
        }


        public IEnumerable LeaveApprovalStatus { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> LeaveApprovalStatusWorkerSelectListItem
        {
            get { return new SelectList(LeaveApprovalStatus, "Id", "Name"); }
        }

        public List<Employee> ApprovalPersons { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ApprovalPersonSelectListItem
        {
            get { return new SelectList(ApprovalPersons, "EmployeeId", "Name"); }
        }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int Year { get; set; }
    }
}