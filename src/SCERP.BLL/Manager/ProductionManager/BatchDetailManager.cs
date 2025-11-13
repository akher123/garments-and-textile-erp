using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
   public class BatchDetailManager: IBatchDetailManager
   {
       private readonly IBatchDetailRepository _batchDetailRepository;

       public BatchDetailManager(IBatchDetailRepository batchDetailRepository)
       {
           _batchDetailRepository = batchDetailRepository;
       }

       public List<VwProdBatchDetail> GetbatchDetailByBatchId(long batchId, string compId)
       {
           return _batchDetailRepository.GetbatchDetailByBatchId(batchId,compId);
       }

   }
}
