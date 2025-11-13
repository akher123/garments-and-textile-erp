using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IRejectReplacementRepository:IRepository<PROD_RejectReplacement>
    {
            List<SpProdJobWiseRejectAdjusment> GetRejectReplacementByCuttingBatch
          (string compId, long cuttingBatchId);
    }
}
