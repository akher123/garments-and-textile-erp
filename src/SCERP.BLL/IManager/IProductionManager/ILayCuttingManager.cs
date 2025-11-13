using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ILayCuttingManager
    {
       List<PROD_LayCutting> GetLayCuttingByCuttingBatchId(long cuttingBatchId);
       int SaveLayCutting(PROD_LayCutting model);
       int DeleteLayCutting(string cuttingBatchRefId, int layCuttingId);
    }
}
