using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IDyeingSpChallanDetailRepository:IRepository<PROD_DyeingSpChallanDetail>
   {
       List<VwProdDyeingSpChallanDetail> GetDyeingSpChallanDetailByDyeingSpChallanId(long dyeingSpChallanId);
       IEnumerable<dynamic> GetBatchItemQtyByBatchDetailId(long batchDetailId, string compId);
   }
}
