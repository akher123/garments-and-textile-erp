using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISkillSetCategoryManager
    {
        List<SkillSetCategory> GetAllSkillSetCategoryByPaging(int startPage, int pageSize, out int totalRecords, SkillSetCategory skillSetCategory);
        bool CheckExistingSkillSetCategory(SkillSetCategory skillSetCategory);
        SkillSetCategory GetSkillSetCategoryById(int? categoryId);
        int SaveSkillSetCategory(SkillSetCategory skillsetcategory);
        int EditSkillSetCategory(SkillSetCategory skillsetcategory);
        int DeleteSkillSetCategory(SkillSetCategory skillsetcategory);
        List<SkillSetCategory> GetAllSkillSetCategory(); 
    }  
}   
   