using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IBatchDetailRepository:IRepository<PROD_BatchDetail>
   {
       List<VwProdBatchDetail> GetbatchDetailByBatchId(long batchId, string compId); 
   }
}
