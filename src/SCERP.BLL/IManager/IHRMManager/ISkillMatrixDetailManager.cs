using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface ISkillMatrixDetailManager
   {
     List<VwSkillMatrix>  GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId);
       List<VwSkillMatrix> GetSkillMatrixByEmployeeId(Guid employeeId, string compId); 
   }
}
