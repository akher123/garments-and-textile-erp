using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.Repository.HRMRepository
{
    public class PenaltyTypeRepository : Repository<HrmPenaltyType>, IPenaltyTypeRepository
    {
        public PenaltyTypeRepository(SCERPDBContext context)
            : base(context)
        {

        }


        public List<HrmPenaltyType> GetAllPenaltyTypes()
        {
            List<HrmPenaltyType> penaltyTypes = Context.PenaltyTypes.Where(r => r.IsActive).OrderBy(x => x.PenaltyTypeId).ToList();
            return penaltyTypes;
        }
    }
}
