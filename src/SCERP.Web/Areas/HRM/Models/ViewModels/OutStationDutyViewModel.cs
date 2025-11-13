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

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class OutStationDutyViewModel : OutStationDuty
    {

        public OutStationDutyViewModel()
        {
            VOutStationDutyDetails=new List<VOutStationDutyDetail>();
            VEmployeeCompanyInfoDetail=new VEmployeeCompanyInfoDetail();
            SearchFieldModel = new SearchFieldModel();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            DepartmentLines = new List<DepartmentLine>();
            DepartmentSections = new List<DepartmentSection>();
            BranchUnitDepartments = new List<object>();
           
        }

        [Display(Name = @"Employee Id")]
        [Required(ErrorMessage  =CustomErrorMessage.RequiredErrorMessage)]
        public string EmployeeCardId { get; set; }
        public List<VOutStationDutyDetail> VOutStationDutyDetails { get; set; }
        public VEmployeeCompanyInfoDetail VEmployeeCompanyInfoDetail { get; set; }  
        public SearchFieldModel SearchFieldModel { get; set; }
       
        //public List<Branch> Branches { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> BranchSelectListItem
        //{
        //    get { return new SelectList(Branches, "Id", "Name"); }
        //}


        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

        }

        //public List<Company> Companies { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> CompanySelectListItem
        //{
        //    get { return new SelectList(Companies, "Id", "Name"); }
        //}

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
    }
}