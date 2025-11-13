using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IMachingInterruptionRepository:IRepository<PROD_MachingInterruption>
    {
       List<SpProdMatchingInterruption> GetMachingInterruptionByDate(DateTime? interrupDate, string processRefId, string compId); 
    }
}
