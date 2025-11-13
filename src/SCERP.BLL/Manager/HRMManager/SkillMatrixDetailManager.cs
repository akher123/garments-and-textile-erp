using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class SkillMatrixDetailManager : ISkillMatrixDetailManager
    {
        private readonly ISkillMatrixDetailRepository _skillMatrixDetailRepository;

        public SkillMatrixDetailManager(ISkillMatrixDetailRepository skillMatrixDetailRepository)
        {
            _skillMatrixDetailRepository = skillMatrixDetailRepository;
        }

        public List<VwSkillMatrix> GetSkillMatrixBySkillMatrixId(int skillMatrixId, string compId)
        {
           IQueryable<VwSkillMatrix> skillMatrixDetailList = _skillMatrixDetailRepository.GetSkillMatrixBySkillMatrixId(skillMatrixId, compId);
           return skillMatrixDetailList.ToList();
        }

        public List<VwSkillMatrix> GetSkillMatrixByEmployeeId(Guid employeeId, string compId)
        {
            IQueryable<VwSkillMatrix> skillMatrixDetailList = _skillMatrixDetailRepository.GetSkillMatrixBySmployeeId(employeeId, compId);
            return skillMatrixDetailList.ToList();
        }
    }
}
