using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class WorkGroupViewModel : WorkGroup
    {

        public WorkGroupViewModel()
        {
            WorkGroups = new List<WorkGroup>();
            IsSearch = true;
            SearchFieldModel = new SearchFieldModel();
        }

        public SearchFieldModel SearchFieldModel { get; set; }

        public List<WorkGroup> WorkGroups { get; set; }

       
        public List<Company> Companies { get; set; }
        public List<SelectListItem> CompanySelectListItem
        {
            get { return new SelectList(Companies, "Id", "Name").ToList(); }

        }

        public List<Branch> Branches { get; set; }
        public List<SelectListItem> BranchSelectListItem
        {
            get { return new SelectList(Branches, "Id", "Name").ToList(); }

        }

        public IEnumerable Units { get; set; }
        public IEnumerable<SelectListItem> UnitSelectListItem
        {
            get { return new SelectList(Units, "BranchUnitId", "UnitName").ToList(); }

        }

        [Required(ErrorMessage = "Required!")]
        public int WorkGroupCompanyId { get; set; }

        [Required(ErrorMessage = "Required!")]
        public int WorkGroupBranchId { get; set; }

        [Required(ErrorMessage = "Required!")]
        public int WorkGroupBranchUnitId { get; set; }
    }
}