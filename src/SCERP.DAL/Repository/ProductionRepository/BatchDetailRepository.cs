using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
   public class BatchDetailRepository:Repository<PROD_BatchDetail>, IBatchDetailRepository
    {
       public BatchDetailRepository(SCERPDBContext context) : base(context)
       {
       }

       public List<VwProdBatchDetail> GetbatchDetailByBatchId(long batchId, string compId)
       {
           return Context.BatchDetail.Where(x => x.CompId == compId && x.BatchId == batchId).ToList();
       }
    }
}
