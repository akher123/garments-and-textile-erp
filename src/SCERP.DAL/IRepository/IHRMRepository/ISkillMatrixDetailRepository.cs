using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface ISkillMatrixDetailRepository:IRepository<HrmSkillMatrixDetail>
    {
       IQueryable<VwSkillMatrix> GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId);
       IQueryable<VwSkillMatrix> GetSkillMatrixBySmployeeId(Guid employeeId, string compId); 
    }
}
