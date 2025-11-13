using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Common;
using SCERP.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class SalaryAdvanceViewModel : SalaryAdvance
    {
        public SalaryAdvanceViewModel()
        {
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            DepartmentLines = new List<DepartmentLine>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            EmployeeTypes = new List<EmployeeType>();
            SalaryAdvances = new List<SalaryAdvance>();
            salaryAdvanceView = new List<SalaryAdvanceView>();
            IsSearch = true;
            EmployeeGuidIdList = new List<Guid>();
            AdvanceAmountList = new List<decimal>();
        }

        public List<SalaryAdvance> SalaryAdvances { get; set; }
        public List<SalaryAdvanceView> salaryAdvanceView { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        public IList<VEmployeeCompanyInfoDetail> Employees { get; set; }

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
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
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

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> FromDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required!")]
        public Nullable<System.DateTime> ToDate { get; set; }

        public string EmployeeCardId { get; set; }

        public bool IsBulkSalaryAdvanceSearch { get; set; }


        public decimal Amount { get; set; }

        public List<Model.Custom.EmployeesForAdvanceSalaryCustomModel> EmployeesForAdvanceSalary { get; set; }

        public List<Guid> EmployeeGuidIdList { get; set; }

        public List<Decimal> AdvanceAmountList { get; set; }

    }

}