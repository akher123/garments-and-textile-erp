using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class DepartmentSectionViewModel:DepartmentSection
    {
        public DepartmentSectionViewModel()
        {
            DepartmentSections = new List<DepartmentSection>();
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Sections = new List<Section>();
            Branches = new List<Branch>();
            IsSearch = true;
        }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IList<Section> Sections { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
        public List<Company> Companies { get; set; }
        public List<Branch> Branches { get; set; }
    
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "Id", "Name"); }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name"); }
        }

    
        public IEnumerable<System.Web.Mvc.SelectListItem> SectionSelectListItem
        {
            get { return new SelectList(Sections, "SectionId", "Name"); }

        }
        public IEnumerable<System.Web.Mvc.SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }

        }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSelectListItem
        {
            get { return new SelectList(BranchUnitDepartments, "BranchUnitDepartmentId", "DepartmentName"); }

        }
    }
}