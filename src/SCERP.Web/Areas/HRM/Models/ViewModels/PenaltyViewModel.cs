
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using iTextSharp.text;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class PenaltyViewModel:HrmPenalty
    {
        public PenaltyViewModel()
        {
            Penalties = new List<VwPenaltyEmployee>();
            PenaltyTypes = new List<HrmPenaltyType>();
            EmployeeCompanyInfo = new VEmployeeCompanyInfoDetail();
        }

        public List<VwPenaltyEmployee> Penalties { get; set; }

        public List<HrmPenaltyType> PenaltyTypes { get; set; }

        public IEnumerable<SelectListItem> PenaltyTypeSelectListItem
        {
            get { return new SelectList(PenaltyTypes, "PenaltyTypeId", "Type"); }
        }

        public VEmployeeCompanyInfoDetail EmployeeCompanyInfo { get; set; }

        [Required]
        public string ClaimerName { get; set; }
        
    }
}