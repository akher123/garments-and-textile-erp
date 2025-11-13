using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface IPartCuttingRepository:IRepository<PROD_PartCutting>
    {
       IQueryable<VwPartCutting> GetVwPartCuttingLsit(string compId, string cuttingBatchRefId);
    }
}
