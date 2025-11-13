using SCERP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.Custom;
using SCERP.Web.Areas.HRM.Models.ViewModels;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class SalaryReportViewModel
    {
        public SalaryReportViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentLines = new List<DepartmentLine>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            ReportHeadsList = GetReportHeads();
        }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        public IList<EmployeeCompanyInfo> EmployeeCompanyInfos { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public List<DepartmentSection> DepartmentSections { get; set; }

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

        public IEnumerable DepartmentLines { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable EmployeeTypes { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeTypeListItem
        {
            get { return new SelectList(EmployeeTypes, "Id", "Title"); }
        }

        public IEnumerable EmployeeGrades { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeGradeListItem
        {
            get { return new SelectList(EmployeeGrades, "Id", "Name"); }
        }

        public IEnumerable EmployeeDesignations { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> EmployeeDesignationListItem
        {
            get { return new SelectList(EmployeeDesignations, "Id", "Title"); }
        }

        public IEnumerable Comparisons { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ComparisonsListItem
        {
            get { return new SelectList(Comparisons, "Text", "Value"); }
        }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeCardId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string SearchByEmployeeName { get; set; }

        public decimal GrossSalary { get; set; }
        public string GrossComparisonId { get; set; }
        public string BasicComparisonId { get; set; }
        public string EffectiveDateComparisonId { get; set; }
        public decimal BasicSalary { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> EffectiveDate { get; set; }


        public List<ReportHead> ReportHeadsList { get; set; }

        private List<ReportHead> GetReportHeads()
        {
            return new List<ReportHead>
            {
                new ReportHead() {Head = "Company", IsSelected = true},
                new ReportHead() {Head = "Branch", IsSelected = true},
                new ReportHead() {Head = "Unit", IsSelected = true},
                new ReportHead() {Head = "Department", IsSelected = true},
                new ReportHead() {Head = "Section", IsSelected = true},
                new ReportHead() {Head = "Line", IsSelected = true},
                new ReportHead() {Head = "EmployeeType", IsSelected = true},
                new ReportHead() {Head = "EmployeeGrade", IsSelected = true},
                new ReportHead() {Head = "Designation", IsSelected = true},
                new ReportHead() {Head = "EmployeeId", IsSelected = true},
                new ReportHead() {Head = "EmployeeName", IsSelected = true},
                new ReportHead() {Head = "GrossSalary", IsSelected = true},
                new ReportHead() {Head = "BasicSalary", IsSelected = true},
                new ReportHead() {Head = "Medical", IsSelected = true},
                new ReportHead() {Head = "HouseRent", IsSelected = true},
                new ReportHead() {Head = "Food", IsSelected = true},
                new ReportHead() {Head = "Conveyence", IsSelected = true},
                new ReportHead() {Head = "EffectiveFrom", IsSelected = true},
                new ReportHead() {Head = "EffectiveTo", IsSelected = true}
            };
        }
    }
}