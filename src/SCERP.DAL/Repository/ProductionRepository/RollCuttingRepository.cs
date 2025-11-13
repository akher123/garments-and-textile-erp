using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class RollCuttingRepository : Repository<PROD_RollCutting>, IRollCuttingRepository
    {
        public RollCuttingRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwRollCutting> GetRollCuttingByCuttingBatchRefId(string compId, string cuttingBatchRefId)
        {
            string sqlQuery = String.Format(@"select R.*,C.ColorName from PROD_RollCutting as R
                                            inner join OM_Color as C on R.ColorRefId=C.ColorRefId and R.CompId=C.CompId
                                       where R.CompId='{0}' and R.CuttingBatchRefId='{1}' order by R.RollCuttingId", compId, cuttingBatchRefId);
            return Context.Database.SqlQuery<VwRollCutting>(sqlQuery).AsQueryable();
        }
    }
}
