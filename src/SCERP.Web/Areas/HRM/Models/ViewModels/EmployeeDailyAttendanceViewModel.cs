using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.Manager.HRMManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class EmployeeDailyAttendanceViewModel : EmployeeDailyAttendance
    {
      
        public EmployeeDailyAttendanceViewModel()
        {
            EmployeeDailyAttendances = new List<VEmployeeDailyAttendanceDetail>();
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            EmployeeCompanyInfo = new VEmployeeCompanyInfoDetail();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            IsSearch = true;
            IsBulkAttendanceSearch = true;
            EmployeeGuidIdList = new List<Guid>();
        }

        public List<Guid> EmployeeGuidIdList { get; set; }
        public VEmployeeCompanyInfoDetail EmployeeCompanyInfo { get; set; }


        public IEnumerable BranchUnits { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? ToDate { get; set; }
        public IList<VEmployeeCompanyInfoDetail> Employees { get; set; }
        public IList<VEmployeeDailyAttendanceDetail> EmployeeDailyAttendances { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
        public bool IsBulkAttendanceSearch { get; set; }

        public String EntryTime { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string HourKey { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string MinuteKey { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string PeriodKey { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> HourSelectListItem
        {
            get { return new SelectList(TimeConfiguration.GetHours(), "HourKey", "Text"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> IsMinuteSelectListItem
        {
            get { return new SelectList(TimeConfiguration.GetMunites(), "MinuteKey", "Text"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> PeriodSelectListItem
        {
            get { return new SelectList(TimeConfiguration.GePeriods(), "PeriodKey", "Text"); }
        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }

        }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }

        }
        public List<DepartmentLine> DepartmentLines { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }
        public DateTime CustomDateTime
        {
            get
            {
                DateTime customDate = DateTime.Now;
                if (Date.HasValue)
                {
                    var time = HourKey + ":" + MinuteKey + " " + PeriodKey;
                    var date = Date.GetValueOrDefault().Date.ToShortDateString() + " " + time;
                    customDate = Convert.ToDateTime(date);
                }
                return customDate;
            }
        }

        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeeCompanyInfo != null ? "" : "none");
            }
        }

        public DateTime GetAttendanceDateTime(int limit, DateTime attendancedate)
        {
            Random random = new Random();
            DateTime customDate = DateTime.Now;
            var time = HourKey + ":" + MinuteKey + " " + PeriodKey;
            var date = attendancedate.Date.ToShortDateString() + " " + time;
            customDate = Convert.ToDateTime(date);
            DateTime maxDateTime = customDate.AddMinutes(random.Next(limit));

            return maxDateTime;
        }

        public List<EmployeeDailyAttendance> GetSelectedEmployeeDailyAttendances()
        {
            var random = new Random();

            var employeeDailyAttendanceList = (from employeeId in EmployeeGuidIdList.ToList()
                                               from date in DateTimeExtension.ToDays(FromDate, ToDate)
                                               select new EmployeeDailyAttendance()
                                               {
                                                   EmployeeId = employeeId,
                                                   Remarks = Remarks,
                                                   TransactionDateTime = Convert.ToDateTime(date.Date.ToShortDateString() + " " + HourKey + ":" + MinuteKey + " " + PeriodKey).AddMinutes(random.Next(Limit)), //GetAttendanceDateTime(Limit, date),
                                                   IsFromMachine = false,
                                                   CreatedBy = PortalContext.CurrentUser.UserId,
                                                   CreatedDate = DateTime.Now,
                                                   IsActive = true,

                                               }).ToList();
            return employeeDailyAttendanceList;
        }
    }
}