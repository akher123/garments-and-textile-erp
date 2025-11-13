using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IRollCuttingManager
    {
       List<VwRollCutting> GetRollCuttingByCuttingBatchRefId(string cuttingBatchRefId);
       int SaveRollCutting(PROD_RollCutting model);
       int DeleteRollCutting(int rollCuttingId, string cuttingBatchRefId);

    }
}
