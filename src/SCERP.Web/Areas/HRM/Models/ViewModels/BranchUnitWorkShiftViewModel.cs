using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;
namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class BranchUnitWorkShiftViewModel : BranchUnitWorkShift
    {
        public BranchUnitWorkShiftViewModel()
        {
            BranchUnitWorkShifts = new List<BranchUnitWorkShift>();
            BranchUnits = new List<object>();
            WorkShifts = new List<WorkShift>();
            SearchFieldModel = new SearchFieldModel();
            Companies = new List<Company>();
            Branches = new List<Branch>();
            IsSearch = true;
        }
        public SearchFieldModel SearchFieldModel { get; set; }

        public List<BranchUnitWorkShift> BranchUnitWorkShifts { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int SerchCompanyId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int SerchBranchId { get; set; }

        public List<Company> Companies { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name"); }

        }
        public List<Branch> Branches { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "Id", "Name"); }

        }
        public IEnumerable BranchUnits { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(BranchUnits, "BranchUnitId", "UnitName"); }

        }

        public List<WorkShift> WorkShifts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> WorkShiftSelectListItem
        {
            get { return new SelectList(WorkShifts, "WorkShiftId", "NameDetail"); }

        }

    }
}