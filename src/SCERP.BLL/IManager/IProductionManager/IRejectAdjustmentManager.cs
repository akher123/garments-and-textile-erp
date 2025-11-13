using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IRejectAdjustmentManager
    {
       Dictionary<string, List<string>> GetRejectAdjustmentByCuttingBatch(long cuttingBatchId);
       int SaveRejectAdjustment(List<PROD_RejectAdjustment> select);

       int DeleteRejectAjsustment(long cuttingBatchId);


    }
}
