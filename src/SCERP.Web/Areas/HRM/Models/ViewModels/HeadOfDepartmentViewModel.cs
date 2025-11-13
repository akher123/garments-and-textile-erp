using System.Linq;
using Org.BouncyCastle.Math.EC;
using SCERP.Model;
using SCERP.Model.Custom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class HeadOfDepartmentViewModel : HeadOfDepartment
    {
        public HeadOfDepartmentViewModel()
        {
            Companies = new List<Company>();
            Branches = new List<Branch>();
            BranchUnits = new List<object>();
            UnitDepartments = new List<object>();
            BranchUnitDepartments = new List<BranchUnitDepartment>();
            SearchFieldModel = new SearchFieldModel();
            IsSearch = true;
        }

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

        public IEnumerable UnitDepartments { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentSelectListItem
        {
            get { return new SelectList(UnitDepartments, "UnitDepartmentId", "DepartmentName"); }

        }

        public List<BranchUnitDepartment> BranchUnitDepartments { get; set; }

        public SearchFieldModel SearchFieldModel { get; set; }      
       
        [Required(ErrorMessage = @"Required!")]
        public int SerchCompanyId { get; set; }

        [Required(ErrorMessage = @"Required!")]
        public int SerchBranchId { get; set; }
    }
}