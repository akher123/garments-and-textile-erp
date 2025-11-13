using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class SkillMatrixProcessRepository : Repository<HrmSkillMatrixProcess>, ISkillMatrixProcessRepository
    {
        public SkillMatrixProcessRepository(SCERPDBContext context) : base(context)
        {
        }
        
        public VwSkillMatrix GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId)
        {
            return Context.VwSkillMatrixs.SingleOrDefault(x => x.CompId == compId && x.SkillMatrixId == skillMatrixId);
        }
    }
}
