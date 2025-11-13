using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProcessReceiveRepository : Repository<PROD_ProcessReceive>, IProcessReceiveRepository
    {
        public ProcessReceiveRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<SpPodProcessReceiveBalance> GetProcessReceiveBalance(string printing, long partyId, string cuttingBatchRefId)
        {
            var parObjects = new[]
            {
                new SqlParameter("@ProcessRefId", printing), new SqlParameter("@PartyId", partyId),
                new SqlParameter("@CompId", PortalContext.CurrentUser.CompId),
                new SqlParameter("@CuttingBatchRefId",cuttingBatchRefId??"NULL")
            };
            return Context.Database.SqlQuery<SpPodProcessReceiveBalance>("SpProdPartyWiseProcessDelivery @ProcessRefId,@PartyId,@CompId,@CuttingBatchRefId", parObjects).ToList();
        }

        public List<SpPodProcessReceiveBalance> GetReceiveBalance(long cuttingBatchId, long cuttingTagId, string printing)
        {
            var parObjects = new[]
            {
                new SqlParameter("@ProcessRefId", printing), new SqlParameter("@CuttingTagId", cuttingTagId),
                new SqlParameter("@CompId", PortalContext.CurrentUser.CompId),
                new SqlParameter("@CuttingBatchId",cuttingBatchId)
            };
            return Context.Database.SqlQuery<SpPodProcessReceiveBalance>("SpProdProcessReceiveBalance @ProcessRefId, @CuttingBatchId,@CuttingTagId,@CompId", parObjects).ToList();
        }
    }
}
