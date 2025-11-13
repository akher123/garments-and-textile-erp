using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IMachingInterruptionManager
    {
       List<SpProdMatchingInterruption> GetMachingInterruptionByDate(DateTime? interrupDate,string processRefId, string compId);
       int SaveMachingInterruption(PROD_MachingInterruption model);
    }
}
