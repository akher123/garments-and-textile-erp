using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class GraddingRepository :Repository<PROD_CuttingGradding>, IGraddingRepository
    {
        public GraddingRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VwCuttingGraddding> GetGradingListByCuttingBatch(long cuttingBatchId, string compId)
        {
            string sqlQuery=string.Format(@"select GR.*,FS.SizeName as FSizeName,TS.SizeName as TSizeName from PROD_CuttingGradding as GR
                            inner join OM_Size as FS on GR.FromSizeRefId=FS.SizeRefId and GR.CompId=FS.CompId
                            inner join OM_Size as TS on GR.ToSizeRefId=TS.SizeRefId and GR.CompId=TS.CompId
                            where GR.CuttingBatchId='{0}' and GR.CompId='{1}'", cuttingBatchId, compId);
            return Context.Database.SqlQuery<VwCuttingGraddding>(sqlQuery).ToList();
        }
    }
}
