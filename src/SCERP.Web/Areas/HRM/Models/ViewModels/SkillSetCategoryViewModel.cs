using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillSetCategoryViewModel : SkillSetCategory
    {
    
        public SkillSetCategoryViewModel()
        {
            SkillSetCategories=new List<SkillSetCategory>();
            IsSearch = true;
        }

        public List<SkillSetCategory> SkillSetCategories { get; set; }
        public string SearchKey
        {
            get;
            set;
        }

    }
}