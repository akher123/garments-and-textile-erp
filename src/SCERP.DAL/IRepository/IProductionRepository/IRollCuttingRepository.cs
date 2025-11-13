using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IRollCuttingRepository:IRepository<PROD_RollCutting>
    {
       IQueryable<VwRollCutting> GetRollCuttingByCuttingBatchRefId(string compId, string cuttingBatchRefId);
    }
}
