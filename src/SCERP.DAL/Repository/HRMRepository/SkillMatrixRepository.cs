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
    public class SkillMatrixRepository : Repository<HrmSkillMatrix>, ISkillMatrixRepository
    {
        public SkillMatrixRepository(SCERPDBContext context) : base(context)
        {
        }
        public IQueryable<VwSkillMatrixEmployee> GetAllSkillMatrixByPaging(string searchString, string compId)
        {
           return Context.VwSkillMatrixEmployees.Where(x => x.CompId == compId
                 &&((x.EmployeeName == searchString || String.IsNullOrEmpty(searchString)) || (x.Designation == searchString || String.IsNullOrEmpty(searchString))));
        }
    }
}
