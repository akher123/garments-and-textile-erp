using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface ISkillMatrixManager 
    {
       List<VwSkillMatrixEmployee> GetAllSkillMatrixByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
       int SaveSkillMatrix(HrmSkillMatrix model);
       int EditSkillMatrix(HrmSkillMatrix model);
       bool IsSkilMatrixExist(Guid employeeId, int skillMatrixId, string compId);
       HrmSkillMatrix GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId);
       int DeleteSkillMatrix(int skillMatrixId, string compId); 
    }
}
