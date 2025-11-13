using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class BranchUnitViewModel : BranchUnit
    {
        public BranchUnitViewModel()
        {
            BranchUnits = new List<BranchUnit>();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            SearchFieldModel = new SearchFieldModel();
            Units=new List<Unit>();

            IsSearch = true;
        }
        [Required(ErrorMessage = @"Required!")]
        public int CompanyId { get; set; }
        public List<Unit> Units { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }
        public List<BranchUnit> BranchUnits { get; set; }
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
        public IEnumerable<System.Web.Mvc.SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(Units, "UnitId", "Name"); }

        }
    }
}