using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class BundleCuttingRepository : Repository<PROD_BundleCutting>, IBundleCuttingRepository
    {
        public BundleCuttingRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public IQueryable<VwBundleCutting> GetVwBundleCuttingByCuttingBatchRefId(string compId, string cuttingBatchRefId)
        {
            string sqlQuery = String.Format(@"SELECT BC.*,OMC.ColorName,OMS.SizeName,COM.ComponentName FROM PROD_BundleCutting AS BC
                    INNER JOIN OM_Color AS OMC
                    ON BC.ColorRefId=OMC.ColorRefId AND BC.CompId=OMC.CompId
                    INNER JOIN OM_Size AS OMS
                    ON BC.SizeRefId=OMS.SizeRefId AND BC.CompId=OMS.CompId
                    INNER JOIN OM_Component AS COM
                    ON BC.ComponentRefId=COM.ComponentRefId AND BC.CompId=COM.CompId
                    where BC.CompId='{0}' AND BC.CuttingBatchRefId='{1}'
                    ", compId, cuttingBatchRefId);
            return Context.Database.SqlQuery<VwBundleCutting>(sqlQuery).AsQueryable();
        }
    }
}
