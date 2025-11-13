using System;
using System.Collections;
using System.Linq;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Planning.Models.ViewModels
{
    public class ProductionLineViewModel : PLAN_ProductionLine
    {
        public ProductionLineViewModel()
        {
            ProductionLines = new List<PLAN_ProductionLine>();  
            SearchFieldModel = new SearchFieldModel();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            Companies = new List<Company>();
            Lines = new List<Line>();
            Branches = new List<Branch>();
            IsSearch = true;
        }
    
        public IEnumerable BranchUnitDepartments { get; set; }
        public IEnumerable BranchUnits { get; set; }
        public List<Line> Lines { get; set; }
        public List<PLAN_ProductionLine> ProductionLines { get; set; }
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
        public IEnumerable<System.Web.Mvc.SelectListItem> LineSelectListItem
        {
            get { return new SelectList(Lines, "LineId", "Name"); }

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