using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IRejectReplacementManager
    {
        Dictionary<string, List<string>> GetRejectReplacementByCuttingBatch(long cuttingBatchId);

        int DeleteRejectReplacement(long cuttingBatchId);

        int SaveRejectReplacement(List<PROD_RejectReplacement> rejectAdjustments);
    }
}
