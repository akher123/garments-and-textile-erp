using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISkillSetDifficultyManager
    {
        List<SkillSetDifficulty> GetAllSkillSetDifficultyByPaging(int startPage, int pageSize, out int totalRecords, SkillSetDifficulty skillSetDifficulty);
        SkillSetDifficulty GetSkillSetDifficultyById(int? skillSetDifficultyId);
        bool CheckExistingSkillSetDifficulty(SkillSetDifficulty skillSetDifficulty);
        int EditSkillSetDifficulty(SkillSetDifficulty skillsetdifficulty);
        int SaveSkillSetDifficulty(SkillSetDifficulty skillsetdifficulty);
        int DeleteSkillSetDifficulty(SkillSetDifficulty skillsetdeleted);
        List<SkillSetDifficulty> GetAllSkillSetDifficulty();
        //List<SkillSetDifficulty> GetAllEmployeeSkillBySearchKey(string searchKey); 
    }
}
 