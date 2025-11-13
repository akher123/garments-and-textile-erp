using System.Collections.Generic;
using SCERP.Model.HRMModel;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IPenaltyTypeRepository : IRepository<HrmPenaltyType>
    {
        List<HrmPenaltyType> GetAllPenaltyTypes();
    }
}
