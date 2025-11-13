using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IBundleCuttingRepository:IRepository<PROD_BundleCutting>
    {
       IQueryable<VwBundleCutting> GetVwBundleCuttingByCuttingBatchRefId(string compId, string cuttingBatchRefId); 
    }
}
