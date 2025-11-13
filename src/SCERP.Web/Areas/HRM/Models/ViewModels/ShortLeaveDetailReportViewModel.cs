using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class ShortLeaveDetailReportViewModel : EmployeeShortLeave
    {
        public ShortLeaveDetailReportViewModel()
        {
            //SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            ReasonTypes = new List<ReasonType>();
            Branches = new List<Branch>();
            IsSearch = true;
        }

        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public EmployeeCompanyInfo EmployeeCompanyInfo { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }

        [Required]
        public string SearchByEmployeeCardId { get; set; }

        public int SearchByCompanyId { get; set; }


        public int SearchByBranchId { get; set; }


        public int SearchByBranchUnitId { get; set; }


        public int SearchByBranchUnitDepartmentId { get; set; }

        public int SearchByDepartmentSectionId { get; set; }

        public int SearchByDepartmentLineId { get; set; }

        [DataType(DataType.Date)]
        //[Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        [Required]
        [Display(Name = @"From date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = @"To date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        //public SearchFieldModel SearchFieldModel { get; set; }
     
        public IEnumerable ReasonTypes { get; set; } 
        
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
        public IEnumerable<System.Web.Mvc.SelectListItem> ReasonTypSelectListItems
        {
            get { return new SelectList(ReasonTypes, "Id", "Name"); }
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
  
    }
}