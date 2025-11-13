using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillSetViewModel : SkillSet
    {
        public SkillSetViewModel()
        {
            SkillSets = new List<SkillSet>();
            IsSearch = true;
        }

        public List<SkillSet> SkillSets { get; set; }
       
        public string SearchKey
        {
            get;
            set;
        }

    }
}