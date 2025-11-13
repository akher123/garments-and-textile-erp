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
    public class EmployeeSkillViewModel : EmployeeSkill
    {

        public EmployeeSkillViewModel()
        {
            EmployeeSkills=new List<EmployeeSkill>();
            
            VEmployeeCompanyInfoDetail = new VEmployeeCompanyInfoDetail();
            VEmployeeSkillDetails=new List<VEmployeeSkillDetail>(); 
            SearchFieldModel = new SearchFieldModel();
            //IsSearch = true;
        }

        [Display(Name = @"Employee Id")]
        public string EmployeeCardId { get; set; }

        public List<EmployeeSkill> EmployeeSkills { get; set; }

        public VEmployeeCompanyInfoDetail VEmployeeCompanyInfoDetail { get; set; }

        public List<VEmployeeSkillDetail> VEmployeeSkillDetails { get; set; }

        public List<SkillSetDifficulty> SkillSetDifficulties { get; set; }

        public List<SelectListItem> SkillSetDifficultySelectListItem
        {
            get { return new SelectList(SkillSetDifficulties, "SkillSetDifficultyId", "DifficultyName").ToList(); }
        }
        public List<SkillSetCategory> SkillSetCategories { get; set; }

        public List<SelectListItem> SkillSetCategorySelectListItem
        {
            get { return new SelectList(SkillSetCategories, "CategoryId", "CategoryName").ToList(); }
        }
     
        public List<SkillOperation> SkillOperations { get; set; } 

        public List<SelectListItem> SkillOperationSelectListItem
        {
            get { return new SelectList(SkillOperations, "SkillOperationId", "Name").ToList(); }
        }

        public int SkillSetDifficultyId { get; set; }
        public int CategoryId { get; set; }

        public int SearchBySkillSetDifficultyId { get; set; }
        public int SearchBySkillCategoryId { get; set; }
        public string SearchByOperationName { get; set; }
        public SearchFieldModel SearchFieldModel { get; set; }      
        public string SearchByEfficiency { get; set; }

      


    }
}