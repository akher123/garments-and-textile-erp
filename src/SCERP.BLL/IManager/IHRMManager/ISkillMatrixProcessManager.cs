using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface ISkillMatrixProcessManager 
    {
       List<HrmSkillMatrixProcess> GetAllSkillMatrixProcess();
       string GetProcessNameBySkillMatrixProcessId(int skillMatrixProcessId, string compId);
       HrmSkillMatrixProcess GetSkillMatrixProcessBySkillMatrixProcessId(int skillMatrixProcessId, string compId);
       bool IsSkillMatrixProcessExist(HrmSkillMatrixProcess model);
       int EditSkillMatrixProcess(HrmSkillMatrixProcess model);
       int SaveSkillMatrixProcess(HrmSkillMatrixProcess model); 
       int DeleteSkillMatrixProcess(int skillMatrixProcessId); 
       List<HrmSkillMatrixProcess> GetAllSkillMatrixProcessByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString);
    }
}
