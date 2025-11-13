using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillOperationViewModel: SkillOperation
    {
        public SkillOperationViewModel()
        {
            SkillOperations=new List<SkillOperation>();
            IsSearch = true;
        }

        public List<SkillOperation> SkillOperations { get; set; }

        public List<SkillSetCategory> SkillSetCategories { get; set; }

        public List<SelectListItem> SkillSetCategorySelectListItem
        {
            get { return new SelectList(SkillSetCategories, "CategoryId", "CategoryName").ToList(); }
        }

        public List<SkillSetDifficulty> SkillSetDifficulties { get; set; }

        public List<SelectListItem> SkillSetDifficultySelectListItem
        {
            get { return new SelectList(SkillSetDifficulties,"SkillSetDifficultyId", "DifficultyName").ToList(); }
        }

        public int SearchBySkillSetDifficultyId { get; set; }

        public int SearchBySkillCategoryId { get; set; } 

        public SearchFieldModel SearchFieldModel { get; set; }

        public string SearchByOperationName { get; set; } 
    }



}