using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IProcessReceiveRepository:IRepository<PROD_ProcessReceive>
    {
        List<SpPodProcessReceiveBalance> GetProcessReceiveBalance(string printing, long partyId, string cuttingBatchRefId);
        List<SpPodProcessReceiveBalance> GetReceiveBalance(long cuttingBatchId, long cuttingTagId, string printing);
    }
}
