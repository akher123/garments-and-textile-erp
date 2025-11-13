using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class StanderdMinValDetailRepository :Repository<PROD_StanderdMinValDetail>, IStanderdMinValDetailRepository
    {
        public StanderdMinValDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwStanderdMinValDetail> GetVwSmvDetails(long standerdMinValueId, string compId)
        {
          return  Context.VwStanderdMinValDetails.Where(x => x.CompId == compId && x.StanderdMinValueId == standerdMinValueId).ToList();
        }
    }
}
