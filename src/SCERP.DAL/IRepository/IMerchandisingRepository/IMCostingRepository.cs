using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IMCostingRepository : IRepository<OM_Costing>
    {
        int UpdateCosting(int costingId, string fieldName, string value);
        OM_Costing GetUpdatedCosting(int costingId);
    }
}
