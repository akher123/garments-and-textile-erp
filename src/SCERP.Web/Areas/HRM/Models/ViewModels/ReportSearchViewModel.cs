using System.Globalization;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ReportSearchViewModel
    {
        public ReportSearchViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentLines = new List<DepartmentLine>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            EmployeeTypes = new List<EmployeeType>();
            AttendanceStatuses = new List<AttendanceStatus>();
            PrintFormatStatuses = new List<PrintFormatType>();
            EmployeeGrades = new List<EmployeeGrade>();
            EmployeeDesignations = new List<EmployeeDesignation>();
            EmployeeBloodGroups = new List<BloodGroup>();
            ReportHeadsList = GetReportHeads();        
            LeaveHistoryReportHeadList = GetLeaveHistoryReportHeads();
            BuyerOrderMasterDataTable = new DataTable();
            ReportDataSource = new DataTable();
            ReportDataSourceTwo = new DataTable();
        }

        public DataTable BuyerOrderMasterDataTable { get; set; }

        public DataTable ReportDataSource { get; set; }
        public DataTable ReportDataSourceTwo { get; set; }

        public List<string> ReportHeads { get; set; }

        public List<ReportHead> ReportHeadsList { get; set; }

        public List<ReportHead> LeaveHistoryReportHeadList { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        public IList<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public IEnumerable Branches { get; set; }

        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable Companies { get; set; }

        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable BranchUnits { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeSelectListItems
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        public IEnumerable BranchUnitDepartments { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "UnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable DepartmentLines { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable DepartmentSections { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeCardId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> FromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> ToDate { get; set; }

        public string InTime { get; set; }
        public string OutTime { get; set; }

        public string EmployeeCardId { get; set; }

        public List<Gender> Genders { get; set; }

        public List<SelectListItem> GenderSelectListItem
        {
            get { return new SelectList(Genders, "GenderId", "Title").ToList(); }
        }

        public int SearchByGenderId { get; set; }

        public IEnumerable AttendanceStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> AttendanceStatusSelectListItems
        {
            get { return new SelectList(AttendanceStatuses, "Id", "Name"); }
        }

        public int AttendanceStatus { get; set; }
        public bool SearchByOTEligibilty { get; set; }
        public bool SearchByExtraOTEligibilty { get; set; }
        public bool SearchByWeekendOTEligibilty { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SelectedMonth { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SelectedToMonth { get; set; }
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

        [Required(ErrorMessage = "Required")]
        public string SelectedToYear { get; set; }

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

        public IEnumerable EmployeeGrades { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeGradeSelectListItems
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }
        }

        public IEnumerable EmployeeDesignations { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeDesignationSelectListItems
        {
            get { return new SelectList(EmployeeDesignations, "Id", "Name"); }
        }

        public IEnumerable EmployeeBloodGroups { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeBloodGroupSelectListItems
        {
            get { return new SelectList(EmployeeBloodGroups, "Id", "GroupName"); }
        }

        public IEnumerable EmployeeReligions { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeReligionSelectListItems
        {
            get { return new SelectList(EmployeeReligions, "ReligionId", "Name"); }
        }

        public IEnumerable EmployeeMaritalStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeMaritalStatusSelectListItems
        {
            get { return new SelectList(EmployeeMaritalStatuses, "MaritalStateId", "Title"); }
        }

        public IEnumerable EmployeeCountries { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeCountrySelectListItems
        {
            get { return new SelectList(EmployeeCountries, "Id", "CountryName"); }
        }

        public IEnumerable EmployeeDistricts { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeDistrictSelectListItems
        {
            get { return new SelectList(EmployeeDistricts, "Id", "Name"); }
        }

        public IEnumerable EmployeeEducationLevels { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeEducationLevelSelectListItems
        {
            get { return new SelectList(EmployeeEducationLevels, "Id", "Title"); }
        }

        public IEnumerable EmployeeStatuses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeStatusSelectListItems
        {
            get { return new SelectList(EmployeeStatuses, "Id", "Name"); }
        }

        private List<ReportHead> GetReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "ActiveStatus", DisplayName = "Active Status", IsSelected = true},
                new ReportHead() {Head = "BasicSalary", DisplayName = "Basic Salary", IsSelected = true},
                new ReportHead() {Head = "BirthDate", DisplayName = "Birth Day", IsSelected = true},
                new ReportHead() {Head = "BirthRegistrationNo", DisplayName = "Birth Registration No", IsSelected = true},
                new ReportHead() {Head = "BloodGroup", DisplayName = "Blood Group", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName = "Branch", IsSelected = true},
                new ReportHead() {Head = "Company", DisplayName = "Company", IsSelected = true},
                new ReportHead() {Head = "ConfirmationDate", DisplayName = "Confirmation Date", IsSelected = true},
                new ReportHead() {Head = "CountryName", DisplayName = "Country", IsSelected = true},
                new ReportHead() {Head = "DistrictName", DisplayName = "District", IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName = "Department", IsSelected = true},
                new ReportHead() {Head = "EducationLevel", DisplayName = "Education Level", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName = "Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName = "Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName = "Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeName", DisplayName = "Employee Name", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName = "Employee Type", IsSelected = true},
                new ReportHead() {Head = "FathersName", DisplayName = "Father's Name", IsSelected = true},
                new ReportHead() {Head = "GenderName", DisplayName = "Gender", IsSelected = true},
                new ReportHead() {Head = "GrossSalary", DisplayName = "Gross Salary", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName = "Joining Date", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName = "Line", IsSelected = true},
                new ReportHead() {Head = "MarriageAnniversaryDate", DisplayName = "Marriage Anniversary Date", IsSelected = true},
                new ReportHead() {Head = "MaritalState", DisplayName = "Marital Status", IsSelected = true},
                new ReportHead() {Head = "MobilePhone", DisplayName = "Mobile No", IsSelected = true},
                new ReportHead() {Head = "MothersName", DisplayName = "Mother's Name", IsSelected = true},
                new ReportHead() {Head = "NationalIdNo", DisplayName = "National Id No", IsSelected = true},
                new ReportHead() {Head = "PunchCardNo", DisplayName = "Punch Card No", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName = "Quit Date", IsSelected = true},
                new ReportHead() {Head = "QuitType", DisplayName = "Quit Type", IsSelected = true},
                new ReportHead() {Head = "ReligionName", DisplayName = "Religion", IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName = "Section", IsSelected = true},
                new ReportHead() {Head = "TaxIdentificationNo", DisplayName = "Tax Identification No", IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName = "Unit", IsSelected = true},
                new ReportHead() {Head = "LastIncrementDate", DisplayName = "Last Increment Date", IsSelected = true},
                new ReportHead() {Head = "LastIncrementAmount", DisplayName = "Last Increment Amount", IsSelected = true},
                new ReportHead() {Head = "PresentAddress", DisplayName = "Present Address", IsSelected = true},
                new ReportHead() {Head = "PermanentAddress", DisplayName = "Permanent Address", IsSelected = true},
                new ReportHead() {Head = "SkillType", DisplayName = "Skill Type", IsSelected = true}
            };
        }

        public int EmployeeCategory { get; set; }

        public IEnumerable EmployeeCategories { get; set; }

        public IEnumerable<SelectListItem> EmployeeCategorySelectListItems
        {
            get { return new SelectList(EmployeeCategories, "Id", "Name"); }
        }

        public int EmployeeActiveStatus { get; set; }

        public IEnumerable EmployeeActiveStatuses { get; set; }

        public IEnumerable<SelectListItem> EmployeeActiveStatusSelectListItems
        {
            get { return new SelectList(EmployeeActiveStatuses, "Id", "Name"); }
        }

        public IEnumerable LeaveTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> LeaveTypeSelectListItems
        {
            get { return new SelectList(LeaveTypes, "Id", "Title"); }
        }

        private List<ReportHead> GetLeaveHistoryReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "ActiveStatus", DisplayName = "Active Status", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName = "Branch", IsSelected = true},
                new ReportHead() {Head = "Company", DisplayName = "Company", IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName = "Department", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName = "Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName = "Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName = "Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeName", DisplayName = "Employee Name", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName = "Employee Type", IsSelected = true},
                new ReportHead() {Head = "GenderName", DisplayName = "Gender", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName = "Joining Date", IsSelected = true},
                new ReportHead() {Head = "LeaveTitle", DisplayName = "Leave Title", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName = "Line", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName = "Quit Date", IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName = "Section", IsSelected = true},
                new ReportHead() {Head = "TotalAllowedLeave", DisplayName = "Total Allowed Leave", IsSelected = true},
                new ReportHead() {Head = "TotalConsumedLeave", DisplayName = "Total Consumed Leave", IsSelected = true},
                new ReportHead() {Head = "ToalAvailableLeave", DisplayName = "Toal Available Leave", IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName = "Unit", IsSelected = true},
                new ReportHead() {Head = "Year", DisplayName = "Year", IsSelected = true},
            };
        }

        public bool All { get; set; }
        public bool Management { get; set; }
        public bool MiddleManagement { get; set; }
        public bool TeamMemberA { get; set; }
        public bool TeamMemberB { get; set; }

        public decimal? OthersDeduction { get; set; }
    }
}