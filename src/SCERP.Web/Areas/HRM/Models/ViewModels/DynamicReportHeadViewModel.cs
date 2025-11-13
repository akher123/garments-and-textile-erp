using System.Globalization;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class DynamicReportHeadViewModel
    {
        public DynamicReportHeadViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentSections = new List<DepartmentSection>();
            DepartmentLines = new List<DepartmentLine>();
            EmployeeTypes = new List<EmployeeType>();

            PrintFormatStatuses = new List<PrintFormatType>();
            ReportHeadsList = GetReportHeads();
            ReportHeadsListCompliance = GetReportHeadsCompliance();
            JobCardModelReportHeaderList = GetJobCardModelReportHeads();
            ReportDataTable = new DataTable();
        }

        public List<string> ReportHeads { get; set; }

        public List<ReportHead> ReportHeadsList { get; set; }

        public List<ReportHead> ReportHeadsListCompliance { get; set; }

        public List<ReportHead> JobCardModelReportHeaderList { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public IEnumerable Companies { get; set; }

        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }

        public IEnumerable BranchUnits { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "UnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable DepartmentSections { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable DepartmentLines { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeCardId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> FromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> ToDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SelectedMonth { get; set; }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                for (int index = 0; index < (DateTimeFormatInfo.InvariantInfo.MonthNames.Length - 1); index++)
                    yield return new SelectListItem
                    {
                        Value = (index + 1).ToString(),
                        Text = DateTimeFormatInfo.InvariantInfo.MonthNames[index]
                    };
            }
        }

        [Required(ErrorMessage = "Required")]
        public string SelectedYear { get; set; }

        public IEnumerable<SelectListItem> Years
        {
            get
            {
                for (int index = 2014; index <= 2030; index++)
                    yield return new SelectListItem
                    {
                        Value = (index).ToString(),
                        Text = (index).ToString()
                    };
            }
        }

        public IEnumerable PrintFormatStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PrintFormatStatusSelectListItems
        {
            get { return new SelectList(PrintFormatStatuses, "Id", "Name"); }
        }

        private List<ReportHead> GetReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "AbsentDays", DisplayName ="Absent Days", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName ="Branch", IsSelected = true},
                new ReportHead() {Head = "Date", DisplayName ="Date", IsSelected = true},
                new ReportHead() {Head = "DayName", DisplayName ="Day Name",IsSelected = true},
                new ReportHead() {Head = "Delay", DisplayName ="Delay",IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName ="Department", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName ="Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName ="Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName ="Employee Type", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName ="Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "ExtraOTHours", DisplayName ="Extra OT Hours",IsSelected = true},
                new ReportHead() {Head = "GrossSalary", DisplayName ="Gross Salary", IsSelected = true},
                new ReportHead() {Head = "Holidays", DisplayName ="Holidays", IsSelected = true},
                new ReportHead() {Head = "HolidayOTHours", DisplayName ="Holiday OT Hours",IsSelected = true},
                new ReportHead() {Head = "InTime", DisplayName ="InTime", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName ="Joining Date", IsSelected = true},
                new ReportHead() {Head = "LateDays", DisplayName ="Late Days", IsSelected = true},
                new ReportHead() {Head = "LeaveDays", DisplayName ="Leave Days", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName ="Line", IsSelected = true},
                new ReportHead() {Head = "LWP", DisplayName ="LWP", IsSelected = true},
                new ReportHead() {Head = "MobileNo", DisplayName ="Mobile No", IsSelected = true},
                new ReportHead() {Head = "Name", DisplayName ="Name", IsSelected = true},
                new ReportHead() {Head = "OSDDays", DisplayName ="OSD Days", IsSelected = true},
                new ReportHead() {Head = "OTHours", DisplayName ="OT Hours",IsSelected = true},
                new ReportHead() {Head = "OTRate", DisplayName ="OT Rate",IsSelected = true},
                new ReportHead() {Head = "OutTime", DisplayName ="OutTime",IsSelected = true},
                new ReportHead() {Head = "PayDays", DisplayName ="Pay Days", IsSelected = true},
                new ReportHead() {Head = "PresentDays", DisplayName ="Present Days", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName ="Quit Date", IsSelected = true},
                new ReportHead() {Head = "Remarks", DisplayName ="Remarks",IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName ="Section", IsSelected = true},
                new ReportHead() {Head = "Shift", DisplayName ="Shift", IsSelected = true},
                new ReportHead() {Head = "Status", DisplayName ="Status", IsSelected = true},
                new ReportHead() {Head = "TotalDays", DisplayName ="Total Days", IsSelected = true},
                new ReportHead() {Head = "TotalExtraOTHours", DisplayName ="Total Extra OTHours",IsSelected = true},
                new ReportHead() {Head = "TotalHolidayOTHours", DisplayName ="Total Holiday OT Hours",IsSelected = true},
                new ReportHead() {Head = "OTHourLast", DisplayName ="Total OT Hours", IsSelected = true},
                new ReportHead() {Head = "TotalPenaltyAttendanceDays", DisplayName ="Total Penalty Attendance Days",IsSelected = true},
                new ReportHead() {Head = "TotalPenaltyFinancialAmount", DisplayName ="Total Penalty Financial Amount",IsSelected = true},
                new ReportHead() {Head = "TotalPenaltyLeaveDays", DisplayName ="Total Penalty Leave Days",IsSelected = true},
                new ReportHead() {Head = "TotalPenaltyOTHours", DisplayName ="Total Penalty OT Hours",IsSelected = true},
                new ReportHead() {Head = "TotalWeekendOTHours", DisplayName ="Total Weekend OT Hours",IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName ="Unit", IsSelected = true},
                new ReportHead() {Head = "WeekendDays", DisplayName ="Weekend Days", IsSelected = true},
                new ReportHead() {Head = "WorkingDays", DisplayName ="Working Days", IsSelected = true},
                new ReportHead() {Head = "WeekendOTHours", DisplayName ="Weekend OT Hours",IsSelected = true},
                new ReportHead() {Head = "LastIncrementDate", DisplayName ="Last Increment Date",IsSelected = true},
                new ReportHead() {Head = "SkillType", DisplayName ="Skill Type",IsSelected = true},
            };
        }

        private List<ReportHead> GetReportHeadsCompliance()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "AbsentDays", DisplayName ="Absent Days", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName ="Branch", IsSelected = true},
                new ReportHead() {Head = "Date", DisplayName ="Date", IsSelected = true},
                new ReportHead() {Head = "DayName", DisplayName ="Day Name",IsSelected = true},
                new ReportHead() {Head = "Delay", DisplayName ="Delay",IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName ="Department", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName ="Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName ="Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName ="Employee Type", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName ="Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "ExtraOTHours", DisplayName ="Extra OT Hours",IsSelected = true},
                new ReportHead() {Head = "GrossSalary", DisplayName ="Gross Salary", IsSelected = true},
                new ReportHead() {Head = "Holidays", DisplayName ="Holidays", IsSelected = true},
                new ReportHead() {Head = "HolidayOTHours", DisplayName ="Holiday OT Hours",IsSelected = true},
                new ReportHead() {Head = "InTime", DisplayName ="InTime", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName ="Joining Date", IsSelected = true},
                new ReportHead() {Head = "LateDays", DisplayName ="Late Days", IsSelected = true},
                new ReportHead() {Head = "LeaveDays", DisplayName ="Leave Days", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName ="Line", IsSelected = true},
                new ReportHead() {Head = "LWP", DisplayName ="LWP", IsSelected = true},
                new ReportHead() {Head = "MobileNo", DisplayName ="Mobile No", IsSelected = true},
                new ReportHead() {Head = "Name", DisplayName ="Name", IsSelected = true},
                new ReportHead() {Head = "OSDDays", DisplayName ="OSD Days", IsSelected = true},
                new ReportHead() {Head = "OTHours", DisplayName ="OT Hours",IsSelected = true},
                new ReportHead() {Head = "OTRate", DisplayName ="OT Rate",IsSelected = true},
                new ReportHead() {Head = "OutTime", DisplayName ="OutTime",IsSelected = true},
                new ReportHead() {Head = "PayDays", DisplayName ="Pay Days", IsSelected = true},
                new ReportHead() {Head = "PresentDays", DisplayName ="Present Days", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName ="Quit Date", IsSelected = true},
                new ReportHead() {Head = "Remarks", DisplayName ="Remarks",IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName ="Section", IsSelected = true},
                new ReportHead() {Head = "Shift", DisplayName ="Shift", IsSelected = true},
                new ReportHead() {Head = "Status", DisplayName ="Status", IsSelected = true},
                new ReportHead() {Head = "TotalDays", DisplayName ="Total Days", IsSelected = true},
                new ReportHead() {Head = "TotalExtraOTHours", DisplayName ="Total Extra OTHours",IsSelected = true},
                new ReportHead() {Head = "TotalHolidayOTHours", DisplayName ="Total Holiday OT Hours",IsSelected = true},
                new ReportHead() {Head = "OTHourLast", DisplayName ="Total OT Hours", IsSelected = true},
                new ReportHead() {Head = "TotalWeekendOTHours", DisplayName ="Total Weekend OT Hours",IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName ="Unit", IsSelected = true},
                new ReportHead() {Head = "WeekendDays", DisplayName ="Weekend Days", IsSelected = true},
                new ReportHead() {Head = "WorkingDays", DisplayName ="Working Days", IsSelected = true},
                new ReportHead() {Head = "WeekendOTHours", DisplayName ="Weekend OT Hours",IsSelected = true},
                new ReportHead() {Head = "LastIncrementDate", DisplayName ="Last Increment Date",IsSelected = true},
                new ReportHead() {Head = "SkillType", DisplayName ="Skill Type",IsSelected = true},
            };
        }

        private List<ReportHead> GetJobCardModelReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "AbsentDays", DisplayName ="Absent Days", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName ="Branch", IsSelected = true},
                new ReportHead() {Head = "Date", DisplayName ="Date", IsSelected = true},
                new ReportHead() {Head = "DayName", DisplayName ="Day Name",IsSelected = true},
                new ReportHead() {Head = "Delay", DisplayName ="Delay",IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName ="Department", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName ="Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName ="Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName ="Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName ="Employee Type", IsSelected = true},
                new ReportHead() {Head = "GrossSalary", DisplayName ="Gross Salary", IsSelected = true},
                new ReportHead() {Head = "Holidays", DisplayName ="Holidays", IsSelected = true},
                new ReportHead() {Head = "InTime", DisplayName ="InTime", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName ="Joining Date", IsSelected = true},
                new ReportHead() {Head = "LateDays", DisplayName ="Late Days", IsSelected = true},
                new ReportHead() {Head = "LeaveDays", DisplayName ="Leave Days", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName ="Line", IsSelected = true},
                new ReportHead() {Head = "LWP", DisplayName ="LWP", IsSelected = true},
                new ReportHead() {Head = "MobileNo", DisplayName ="Mobile No", IsSelected = true},
                new ReportHead() {Head = "Name", DisplayName ="Name", IsSelected = true},
                new ReportHead() {Head = "OSDDays", DisplayName ="OSD Days", IsSelected = true},
                new ReportHead() {Head = "OTHours", DisplayName ="OT Hours",IsSelected = true},
                new ReportHead() {Head = "OTRate", DisplayName ="OT Rate",IsSelected = true},
                new ReportHead() {Head = "OutTime", DisplayName ="OutTime",IsSelected = true},
                new ReportHead() {Head = "PayDays", DisplayName ="Pay Days", IsSelected = true},
                new ReportHead() {Head = "PresentDays", DisplayName ="Present Days", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName ="Quit Date", IsSelected = true},
                new ReportHead() {Head = "Remarks", DisplayName ="Remarks",IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName ="Section", IsSelected = true},
                new ReportHead() {Head = "Shift", DisplayName ="Shift", IsSelected = true},
                new ReportHead() {Head = "Status", DisplayName ="Status", IsSelected = true},
                new ReportHead() {Head = "TotalDays", DisplayName ="Total Days", IsSelected = true},
                new ReportHead() {Head = "OTHourLast", DisplayName ="Total OT Hours", IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName ="Unit", IsSelected = true},
                new ReportHead() {Head = "WeekendDays", DisplayName ="Weekend Days", IsSelected = true},
                new ReportHead() {Head = "WorkingDays", DisplayName ="Working Days", IsSelected = true}
            };
        }

        public DataTable ReportDataTable { get; set; }
    }
}