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
    public class SkillMatrixDetailRepository : Repository<HrmSkillMatrixDetail>, ISkillMatrixDetailRepository
    {
        public SkillMatrixDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwSkillMatrix> GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId)
        {
            return
                Context.VwSkillMatrixs.Where(
                    x => x.IsActive == true && x.CompId == compId && x.SkillMatrixId == skillMatrixId);
        }

        public IQueryable<VwSkillMatrix> GetSkillMatrixBySmployeeId(Guid employeeId, string compId)
        {
            return
                Context.VwSkillMatrixs.Where(
                    x => x.IsActive == true && x.CompId == compId && x.EmployeeId == employeeId);
        }
    }
}
