using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IStanderdMinValueRepository:IRepository<PROD_StanderdMinValue>
    {
        string GetStanderdMinValueRefId(string compId);
    }
}
