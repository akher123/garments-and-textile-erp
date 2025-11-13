using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IRejectAdjustmentRepository:IRepository<PROD_RejectAdjustment>
    {
       List<SpProdJobWiseRejectAdjusment> GetRejectAdjustmentByCuttingBatch
           (string compId, long cuttingBatchId);
    }
}
