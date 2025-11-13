using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IBatchRepository:IRepository<Pro_Batch>
   {
       IQueryable<VProBatch> GetBachList();
       string GetNewBtRefNo(string prefix);
       List<VwProdBatchDetail> GetBachByBatchId(long batchId, string compId);
   }
}
