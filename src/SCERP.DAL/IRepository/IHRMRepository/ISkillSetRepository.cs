using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface ISkillSetRepository:IRepository<SkillSet>
    {
        SkillSet GetSkillSetById(int? id);
        List<SkillSet> GetAllSkillSetsByPaging(int startPage, int pageSize,out int totalRecords, SkillSet skillSet);
        bool SkillSetIsExist(SkillSet skillSet);
    }
}
