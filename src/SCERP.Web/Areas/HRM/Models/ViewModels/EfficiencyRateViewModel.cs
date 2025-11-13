using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EfficiencyRateViewModel: EfficiencyRate
    {

        public EfficiencyRateViewModel()
        {
            
           EfficiencyRates=new List<EfficiencyRate>();
           SearchFieldModel = new SearchFieldModel();
            //IsActive = true;
        }
    
        public List<EfficiencyRate> EfficiencyRates { get; set; }

        public List<SkillOperation> SkillOperations { get; set; }

        public List<SelectListItem> SkillOperationSelectListItem
        {

            get { return new SelectList(SkillOperations, "SkillOperationId", "Name").ToList(); }
        }

        public int SearchBySkillOperationId { get; set; }
        public string SearchByRate { get; set; } 
        public SearchFieldModel SearchFieldModel { get; set; }
       

    }
}