using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IPartCuttingManager
    {
       List<VwPartCutting> GetAllPartCutting(string cuttingBatchRefId);
       int SavePartCutting(PROD_PartCutting model);

       int DeletePartCutting(int partCuttingId, string cuttingBatchRefId);
       PROD_PartCutting GetPartCuttingByCuttingBatchId(long cuttingBatchId);
    }
}
