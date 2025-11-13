using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IGraddingRepository:IRepository<PROD_CuttingGradding>
    {
        List<VwCuttingGraddding> GetGradingListByCuttingBatch(long cuttingBatchId, string compId);
    }
}
