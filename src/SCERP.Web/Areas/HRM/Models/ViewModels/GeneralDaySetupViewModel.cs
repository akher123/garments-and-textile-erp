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
    public class GeneralDaySetupViewModel : GeneralDaySetup
    {
        public GeneralDaySetupViewModel()
        {
            GeneralDaySetups = new List<GeneralDaySetup>();
            SearchFieldModel = new SearchFieldModel();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
            BranchUnitDepartments = new List<object>();
            IsSearch = true;         
        }

        public List<GeneralDaySetup> GeneralDaySetups { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }

        public IEnumerable Companies { get; set; }
        public IEnumerable<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "CompanyId", "CompanyName").ToList(); }

        }

        public IEnumerable Branches { get; set; }
        public IEnumerable<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "BranchId", "BranchName").ToList(); }

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
            
    }
}