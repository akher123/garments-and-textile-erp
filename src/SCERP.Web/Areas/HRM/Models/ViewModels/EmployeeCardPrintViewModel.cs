using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Common;
using SCERP.Model.Custom;
using System.Web.Mvc;
using System.Collections;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeCardPrintViewModel : Employee
    {
        public EmployeeCardPrintViewModel()
        {
            EmployeeCardInfos = new List<EmployeeCardPrintModel>();
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Sections = new List<Section>();
            Branches = new List<Branch>();
            EmployeeIdList = new List<Guid>();
            IsSearch = true;
        }
        public List<Guid> EmployeeIdList { get; set; } 
        public List<DepartmentLine> DepartmentLines { get; set; }
        public List<DepartmentSection> DepartmentSections { get; set; }
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public IList<Section> Sections { get; set; }
       
        public List<EmployeeCardPrintModel> EmployeeCardInfos { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }           
        public IEnumerable LanguageTypes { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string NoOfCard { get; set; }

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

        public IEnumerable<System.Web.Mvc.SelectListItem> LanguageSelectListItem
        {
            get { return new SelectList(LanguageTypes, "Id", "Name"); }
        }
         
    }
}