using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IBatchRollManager
    {
       List<VwBatchRoll> GetBatchRollByBatchId(long batchId);
       PROD_BatchRoll GetBatchRollById(long batchId, long knittingRollId);
       int SaveBatchRoll(List<PROD_BatchRoll> batchRolls);

       VwBatchRoll GetBatchRollById(long p);
       int DeleteRoll(long batchRollId);
    }
}
