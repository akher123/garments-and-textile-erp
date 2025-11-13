using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IGraddingManager
    {
       int SaveGradding(PROD_CuttingGradding gradding);
       int EditGradding(PROD_CuttingGradding gradding);
       List<VwCuttingGraddding> GetGradingListByCuttingBatch(long cuttingBatchId);
       PROD_CuttingGradding GetGraddingById(long cuttingGraddingId);
       int DeleteGradding(long cuttingGraddingId);
    }
}
