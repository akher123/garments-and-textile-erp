using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class PartCuttingRepository : Repository<PROD_PartCutting>, IPartCuttingRepository
    {
        public PartCuttingRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwPartCutting> GetVwPartCuttingLsit(string compId, string cuttingBatchRefId)
        {
            string sqlQuery =String.Format(@"select P.*,C.ComponentName from PROD_PartCutting as P " +
                              "inner join OM_Component as C on P.ComponentRefId=C.ComponentRefId where " +
                              "P.CompId='{0}' and  P.CuttingBatchRefId='{1}'", compId, cuttingBatchRefId);
            return Context.Database.SqlQuery<VwPartCutting>(sqlQuery).AsQueryable();
        }
    }
}
