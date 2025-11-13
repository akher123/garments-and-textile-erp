using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IBatchRollRepository:IRepository<PROD_BatchRoll>
   {
       List<VwBatchRoll> GetBatchRollByBatchId(long batchId);
       VwBatchRoll GetBatchRollById(long batchRollId);
   }
}
