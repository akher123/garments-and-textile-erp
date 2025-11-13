using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISkillSetManager
    {
        List<SkillSet> GetAllSkillSetsByPaging(int startPage, int pageSize, out int totalRecords,SkillSet skillSet);

        SkillSet GetSkillSetById(int? id);

        int SaveSkillSet(SkillSet skillset);

        int EditSkillSet(SkillSet skillset);
        
        int DeleteSkillSet(SkillSet skillset);


        bool IsExistSkillSets(SkillSet skillSet);

        List<SkillSet> GetSkillSetBySearchKey(string searchKey);
    }
}
