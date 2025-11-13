using System.Globalization;
using SCERP.Model;
using SCERP.Model.Custom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Areas.HRM.Models.ViewModels;
using SCERP.Common;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
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

            ReportHeadsList = GetReportHeads();
        }
        public ReportType ReportType { get; set; }
        public DateTime? PrintedDate { get; set; }
        public List<string> ReportHeads { get; set; }
        public List<ReportHead> ReportHeadsList { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }
   
        public SearchFieldModel SearchFieldModel { get; set; }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName"); }

        }
      
        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName",PortalContext.CurrentUser.CompanyId); }

        }

        public IEnumerable BranchUnits { get; set; }
        public IEnumerable<SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable EmployeeTypes { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeSelectListItems
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable<SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "UnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable DepartmentSections { get; set; }
        public IEnumerable<SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable DepartmentLines { get; set; }
        public IEnumerable<SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeGrades { get; set; }
        public IEnumerable<SelectListItem> EmployeeGradeSelectListItems
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }
        }

        public IEnumerable EmployeeDesignations { get; set; }
        public IEnumerable<SelectListItem> EmployeeDesignationSelectListItems
        {
            get { return new SelectList(EmployeeDesignations, "Id", "Name"); }
        }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeCardId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required!")]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required!")]
        public DateTime? ToDate { get; set; }

        public string EmployeeCardId { get; set; }

        public List<Gender> Genders { get; set; }
        public List<SelectListItem> GenderSelectListItem
        {
            get { return new SelectList(Genders, "GenderId", "Title").ToList(); }
        }
        public int SearchByGenderId { get; set; }

     
        public IEnumerable EmployeeCategories { get; set; }
        public IEnumerable<SelectListItem> EmployeeCategorySelectListItems
        {
            get { return new SelectList(EmployeeCategories, "Id", "Name"); }
        }


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

        public IEnumerable<System.Web.Mvc.SelectListItem> MonthsSelectListItems
        {
            get { return new SelectList(Months, "Value", "Text",DateTime.Now.Month); }
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
        public IEnumerable<System.Web.Mvc.SelectListItem> YearsSelectListItems
        {
            get { return new SelectList(Years, "Value", "Text", DateTime.Now.Year); }
        }
        public int EmployeeCategory { get; set; }


        public IEnumerable PrintFormatStatuses { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PrintFormatStatusSelectListItems
        {
            get { return new SelectList(PrintFormatStatuses, "Id", "Name"); }
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
                new ReportHead() {Head = "ActiveStatus", DisplayName ="Active Status", IsSelected = true},
                new ReportHead() {Head = "BasicSalary", DisplayName ="Basic Salary", IsSelected = true},
                new ReportHead() {Head = "Branch", DisplayName ="Branch", IsSelected = true},
                new ReportHead() {Head = "Company", DisplayName ="Company", IsSelected = true},
                new ReportHead() {Head = "Conveyance", DisplayName ="Conveyance", IsSelected = true},
                new ReportHead() {Head = "Department", DisplayName ="Department", IsSelected = true},
                new ReportHead() {Head = "EmployeeCardId", DisplayName ="Employee Card Id", IsSelected = true},
                new ReportHead() {Head = "EmployeeDesignation", DisplayName ="Employee Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", DisplayName ="Employee Grade", IsSelected = true},
                new ReportHead() {Head = "EmployeeName", DisplayName ="Employee Name", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", DisplayName ="Employee Type", IsSelected = true},
                new ReportHead() {Head = "EntertainmentAllowance", DisplayName ="Entertainment Allowance", IsSelected = true},
                new ReportHead() {Head = "EffectiveFromDate", DisplayName ="Effective From", IsSelected = true},
                new ReportHead() {Head = "FoodAllowance", DisplayName ="Food Allowance", IsSelected = true},
                new ReportHead() {Head = "GenderName", DisplayName ="Gender", IsSelected = true},
                new ReportHead() {Head = "GrossSalary", DisplayName ="Gross Salary", IsSelected = true},
                new ReportHead() {Head = "HouseRent", DisplayName ="House Rent", IsSelected = true},
                new ReportHead() {Head = "JoiningDate", DisplayName ="Joining Date", IsSelected = true},
                new ReportHead() {Head = "Line", DisplayName ="Line", IsSelected = true},
                new ReportHead() {Head = "MedicalAllowance", DisplayName ="Medical Allowance", IsSelected = true},
                new ReportHead() {Head = "QuitDate", DisplayName ="Quit Date", IsSelected = true},
                new ReportHead() {Head = "Section", DisplayName ="Section", IsSelected = true},
                new ReportHead() {Head = "Unit", DisplayName ="Unit", IsSelected = true},                                                          
            };
        }

    }
}