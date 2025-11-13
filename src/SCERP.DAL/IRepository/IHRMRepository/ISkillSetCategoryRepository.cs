using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface ISkillSetCategoryRepository:IRepository<SkillSetCategory>
   {
       List<SkillSetCategory> GetAllSkillSetCategoryByPaging(int startPage, int pageSize, out int totalRecords, SkillSetCategory skillsetCategory);

       SkillSetCategory GetSkillSetCategoryById(int? categoryId);
       List<SkillSetCategory> GetAllSkillSetCategory();
   }
}
 