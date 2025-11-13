using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface ISkillMatrixGradeManager 
    {
        HrmSkillMatrixGrade GetGradeNameByProcessPercentage(int processPercentage, string compId);
        List<HrmSkillMatrixGrade> GetAllSkillMatrixGradeByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString);
        HrmSkillMatrixGrade GetSkillMatrixGradeBySkillMatrixGradeId(int skillMatrixGradeId, string compId);
        bool IsSkillMatrixGradeExist(HrmSkillMatrixGrade model);
        int EditSkillMatrixGrade(HrmSkillMatrixGrade model);
        int SaveSkillMatrixGrade(HrmSkillMatrixGrade model);
        int DeleteSkillMatrixGrade(int skillMatrixGradeId); 
    }
}
