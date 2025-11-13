using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class RejectReplacementRepository :Repository<PROD_RejectReplacement>, IRejectReplacementRepository
    {
        public RejectReplacementRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpProdJobWiseRejectAdjusment> GetRejectReplacementByCuttingBatch(string compId, long cuttingBatchId)
        {
            SqlParameter cuttingBatchIdSp = new SqlParameter("@CuttingBatchId", cuttingBatchId);
            SqlParameter copIdSp = new SqlParameter("@CompId", compId);
            var spList = new[] { cuttingBatchIdSp, copIdSp };
            return Context.Database.SqlQuery<SpProdJobWiseRejectAdjusment>("SpProdJobWiseRejectReplacement @CuttingBatchId,@CompId", spList).ToList();
            
        }
    }
}
