using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillSetDifficultyViewModel:SkillSetDifficulty
    {

        public SkillSetDifficultyViewModel()
        {
            SkillSetDifficulties=new List<SkillSetDifficulty>();
            SearchFieldModel = new SearchFieldModel();
            //IsSearch = true;
         
        }

        public List<SkillSetDifficulty> SkillSetDifficulties { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }  
        public string SearchKey
        {
            get;
            set;
        }
    }
}