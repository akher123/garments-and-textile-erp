using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class AbsentOtPenaltyViewModel : HrmAbsentOTPenalty
    {
        public AbsentOtPenaltyViewModel()
        {
            EmployeeCompanyInfos = new List<VEmployeeCompanyInfoDetail>();
            OvertimeEligibleEmployeeDetails = new List<VOvertimeEligibleEmployeeDetail>();
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            EmployeeGuidIdList = new List<Guid>();
            Branches = new List<Branch>();
            DepartmentLines = new List<DepartmentLine>();
            DepartmentSections = new List<DepartmentSection>();
            IsSearch = true;
            IsAssigneeSerached = true;
        }

        public List<Guid> EmployeeGuidIdList { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required!")]
        public DateTime? FromPenaltyDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required!")]
        public DateTime? ToOvertimeDate { get; set; }

        public IList<VEmployeeCompanyInfoDetail> EmployeeCompanyInfos { get; set; }

        public IList<SPGetAbsentOtPenaltyEmployee> PenaltyEmployees { get; set; }

        public List<OvertimeEligibleEmployee> OvertimeEligibleEmployees { get; set; }

        public List<VOvertimeEligibleEmployeeDetail> OvertimeEligibleEmployeeDetails { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public bool IsAssigneeSerached { get; set; }

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

        public IEnumerable<System.Web.Mvc.SelectListItem> BranchUnitDepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }
        }

        public string SubmitButtonDisplayStyle
        {
            get { return String.Format("style=display:{0}", PenaltyEmployees.Count > 0 ? "" : "none"); }
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

        public List<HrmAbsentOTPenalty> AbsentPenaltyEmployee()
        {
            var overtimeEligibleEmployeeList = (from employeeId in EmployeeGuidIdList

                select new HrmAbsentOTPenalty()
                {
                    EmployeeId = employeeId,
                }).ToList();
            return overtimeEligibleEmployeeList;
        }
    }
}