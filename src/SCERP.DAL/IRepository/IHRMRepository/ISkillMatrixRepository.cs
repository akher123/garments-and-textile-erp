using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
   public interface ISkillMatrixRepository:IRepository<HrmSkillMatrix>
    { 
       IQueryable<VwSkillMatrixEmployee> GetAllSkillMatrixByPaging(string searchString, string compId); 
    }
}
