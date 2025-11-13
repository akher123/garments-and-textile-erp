using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ITargetProductionDetailRepository:IRepository<PLAN_TargetProductionDetail>
   {
       List<SpPlaningTargetProductionDetail> GetSpPlaningTargetProductionDetail();
       int SaveTargetDetail(PLAN_TargetProduction targetProduction);
   }
}
