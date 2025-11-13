using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class AttendanceViewModel:EmployeeDailyAttendance
    {
        public AttendanceViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            AttendanceStatuses = new List<AttendanceStatus>();
            Branches = new List<Branch>();
            WorkShifts = new List<WorkShift>();
            EmployeeTypes = new List<EmployeeType>();
            PrintFormatStatuses = new List<PrintFormatType>();
            IsSearch = true;
            ReportHeadsList = GetReportHeads();
            ModelReportHeadsList = GetModelReportHeads();
        }

        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }

        public List<string> ReportHeads { get; set; } 
        public List<ReportHead> ReportHeadsList { get; set; }

        public List<string> ModelReportHeads { get; set; }
        public List<ReportHead> ModelReportHeadsList { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? EndDate { get; set; }

        
        public int SearchByCompanyId { get; set; }
        public int SearchByBranchId { get; set; }
        public int SearchByBranchUnitId { get; set; }
        public int SearchByBranchUnitDepartmentId { get; set; }


        public string SearchByEmployeeCardId { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }
      
        
    
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }  

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable AttendanceStatuses { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AttendanceStatusSelectListItems
        {
            get { return new SelectList(AttendanceStatuses, "Id", "Name"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "UnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public string SubmitButtonDisplayStyle
        {
            get
            {
                return String.Format("style=display:{0}", EmployeeCompanyInfo != null ? "" : "none");
            }
        }

        public List<WorkShift> WorkShifts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> WorkShiftSelectListItem
        {
            get { return new SelectList(WorkShifts, "ValueMember", "DisplayMember"); }
        }


        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeSelectListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title").ToList(); }

        }

        public bool SearchByOTEligibilty { get; set; }

        public bool SearchByExtraOTEligibilty { get; set; }

        public bool SearchByWeekendOTEligibilty { get; set; }

        public int AttendanceStatus { get; set; }

        public IEnumerable PrintFormatStatuses { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PrintFormatStatusSelectListItems
        {
            get { return new SelectList(PrintFormatStatuses, "Id", "Name"); }
        }

        [Required(ErrorMessage = @"Required")]
        public int SearchByBranchUnitWorkShiftId { get; set; }

        public string TotalContinuousAbsentDays { get; set; }
        public IEnumerable<SelectListItem> NumberOfDaysSelectListItems
        {
            get
            {
                for (int index = 0; index <= 31; index++)
                    yield return new SelectListItem
                    {
                        Value = (index).ToString(),
                        Text = (index).ToString()
                    };
            }
        }

        private List<ReportHead> GetReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "Date", DisplayName = "Date", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName = "Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName = "Employee Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName = "Employee Type",IsSelected = true},
                new ReportHead() {Head = "ExtraOTHours", DisplayName = "Extra OT Hours", IsSelected = true},
                new ReportHead() {Head = "LastDayExtraOTHours", DisplayName = "Extra OT Hours (Last Day)", IsSelected = true},
                new ReportHead() {Head = "HolidayOTHours", DisplayName = "Holiday OT Hours", IsSelected = true},
                new ReportHead() {Head = "InTime", DisplayName = "In Time",IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName = "Joining Date", IsSelected = true},
                new ReportHead() {Head = "LateTime", DisplayName = "Late Time", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName = "Line", IsSelected = true},
                new ReportHead() {Head = "MobileNo", DisplayName = "Mobile No",IsSelected = true},
                new ReportHead() {Head = "EmployeeName", DisplayName = "Name",IsSelected = true},  
                new ReportHead() {Head = "OTHours", DisplayName = "OT Hours", IsSelected = true},
                new ReportHead() {Head = "LastDayOTHours", DisplayName = "OT Hours (Last Day)", IsSelected = true},
                new ReportHead() {Head = "OutTime", DisplayName = "Out Time", IsSelected = true},
                new ReportHead() {Head = "LastDayOutTime", DisplayName = "Out Time (Last Day)", IsSelected = true},
                new ReportHead() {Head = "Remarks", DisplayName = "Remarks",IsSelected = true},  
                new ReportHead() {Head = "Section", DisplayName = "Section", IsSelected = true},
                new ReportHead() {Head = "SignatureOfEmployee", DisplayName = "Sign",IsSelected = true},   
                new ReportHead() {Head = "Status", DisplayName = "Status", IsSelected = true}, 
                new ReportHead() {Head = "TotalContinuousAbsentDays", DisplayName = "Total Continuous Absent Days", IsSelected = true},
                new ReportHead() {Head = "WeekendOTHours", DisplayName = "Weekend OT Hours", IsSelected = true}, 
                new ReportHead() {Head = "WorkShiftName", DisplayName = "Work Shift", IsSelected = true},
                new ReportHead() {Head = "LastPresentDate", DisplayName = "Last Present Date", IsSelected = true}
            };
        }

        private List<ReportHead> GetModelReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "Date", DisplayName = "Date", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName = "Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName = "Employee Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName = "Employee Type",IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName = "Joining Date", IsSelected = true},
                new ReportHead() {Head = "InTime", DisplayName = "In Time",IsSelected = true},
                new ReportHead() {Head = "LateTime", DisplayName = "Late Time", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName = "Line", IsSelected = true},
                new ReportHead() {Head = "MobileNo", DisplayName = "Mobile No",IsSelected = true},
                new ReportHead() {Head = "EmployeeName", DisplayName = "Name",IsSelected = true},  
                new ReportHead() {Head = "OTHours", DisplayName = "OT Hours", IsSelected = true},
                new ReportHead() {Head = "LastDayOTHours", DisplayName = "OT Hours (Last Day)", IsSelected = true},
                new ReportHead() {Head = "OutTime", DisplayName = "Out Time", IsSelected = true},
                new ReportHead() {Head = "LastDayOutTime", DisplayName = "Out Time (Last Day)", IsSelected = true},
                new ReportHead() {Head = "Remarks", DisplayName = "Remarks",IsSelected = true},  
                new ReportHead() {Head = "Section", DisplayName = "Section", IsSelected = true},
                new ReportHead() {Head = "SignatureOfEmployee", DisplayName = "Sign",IsSelected = true},   
                new ReportHead() {Head = "Status", DisplayName = "Status", IsSelected = true}, 
                new ReportHead() {Head = "TotalContinuousAbsentDays", DisplayName = "Total Continuous Absent Days", IsSelected = true},
                new ReportHead() {Head = "WorkShiftName", DisplayName = "Work Shift", IsSelected = true}                                                        
            };
        }

    }
}