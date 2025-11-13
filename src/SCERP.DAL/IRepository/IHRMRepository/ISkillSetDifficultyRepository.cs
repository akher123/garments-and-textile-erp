using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface ISkillSetDifficultyRepository : IRepository<SkillSetDifficulty>
    {
       List<SkillSetDifficulty> GetAllSkillSetDifficultyByPaging(int startPage, int pageSize, out int totalRecords, SkillSetDifficulty skillSetDifficulty);
       SkillSetDifficulty GetSkillSetDifficultyById(int? skillSetDifficultyId);
       List<SkillSetDifficulty> GetAllSkillSetDifficulty();
       List<SkillSetDifficulty> GetAllEmployeeSkillBySearchKey(string searchKey); 
    }
}
  