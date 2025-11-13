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
    public class RejectAdjustmentRepository : Repository<PROD_RejectAdjustment>, IRejectAdjustmentRepository
    {
        public RejectAdjustmentRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpProdJobWiseRejectAdjusment> GetRejectAdjustmentByCuttingBatch(string compId, long cuttingBatchId)
        {
            SqlParameter cuttingBatchIdSp = new SqlParameter("@CuttingBatchId", cuttingBatchId);
            SqlParameter copIdSp = new SqlParameter("@CompId", compId);
            var spList = new[] { cuttingBatchIdSp, copIdSp };
            return Context.Database.SqlQuery<SpProdJobWiseRejectAdjusment>("SpProdJobWiseRejectAdjusment @CuttingBatchId,@CompId", spList).ToList();
            
        }
    }
}
