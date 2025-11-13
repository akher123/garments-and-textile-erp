using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model;
using System.Web.Mvc;
using System.Collections;
using SCERP.Model.Custom;
using SCERP.Common;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class EmployeeSalarySearchViewModel : EmployeeSalary_Processed
    {
        public EmployeeSalarySearchViewModel()
        {
            EmployeeSalaries = new List<EmployeeSalarySearchModel>();
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Sections = new List<Section>();
            Branches = new List<Branch>();
            IsSearch = true;
        }

        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IList<Section> Sections { get; set; }
        public List<EmployeeSalarySearchModel> EmployeeSalaries { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
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

        public IEnumerable<System.Web.Mvc.SelectListItem> SectionSelectListItem
        {
            get { return new SelectList(Sections, "SectionId", "Name"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentLineSelectListItem
        {
            get { return new SelectList(DepartmentLines, "ValueMember", "DisplayMember"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSectionSelectListItem
        {
            get { return new SelectList(DepartmentSections, "ValueMember", "DisplayMember"); }
        }

        [Required(ErrorMessage = @"Required")]
        [DataType(DataType.Date)]
        [Display(Name = @"To date")]
        public DateTime? FromDateCustom { get; set; }

        [Required(ErrorMessage = @"Required")]
        [DataType(DataType.Date)]
        [Display(Name = @"To date")]
        public DateTime? ToDateCustom { get; set; }

    }
}
