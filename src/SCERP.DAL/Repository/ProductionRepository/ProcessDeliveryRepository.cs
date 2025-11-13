using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class ProcessDeliveryRepository:Repository<PROD_ProcessDelivery>,IProcessDeliveryRepository
    {
        public ProcessDeliveryRepository(SCERPDBContext context) : base(context)
        {

        }
        

        public List<OM_Buyer> GetBuyerByPartyId(long partyId, string compId)
        {

            var sqlQuery =string.Format(@"select distinct B.* from PROD_CuttingTagSupplier as CS 
                              left join PROD_CuttingTag as CT on CS.CuttingTagId=CT.CuttingTagId and CS.CompId=CT.CompId
                              left join PROD_CuttingSequence as CSQ on CT.CuttingSequenceId=CSQ.CuttingSequenceId and CSQ.CompId=CT.CompId
                              left join OM_Buyer as B on CSQ.BuyerRefId=B.BuyerRefId and CSQ.CompId=B.CompId 
                               where CS.PartyId='{0}' and CS.CompId='{1}' order by B.BuyerName", partyId,compId);
            return Context.Database.SqlQuery<OM_Buyer>(sqlQuery).ToList();
        }

        public IQueryable<VwProcessDelivery> GetProcessDelivery(string processRefId, string compId)
        {
            return Context.VwProcessDeliveries.Where(x => x.CompId == compId && x.ProcessRefId == processRefId);
        }

        public List<SpProdJobWiseRejectAdjusment> GetJobWiserBalanceDelivery(string compId, long cuttingBatchId,long cuttingTagId, string processRefId)
        {
            SqlParameter cuttingBatchIdSp = new SqlParameter("@CuttingBatchId", cuttingBatchId);
            SqlParameter copIdSp = new SqlParameter("@CompId", compId);
            SqlParameter processRefIdSp = new SqlParameter("@ProcessRefId", processRefId);
            SqlParameter cuttingTagSp = new SqlParameter("@CuttingTagId", cuttingTagId);
            var spList = new[] { cuttingBatchIdSp, copIdSp, processRefIdSp, cuttingTagSp };
            return Context.Database.SqlQuery<SpProdJobWiseRejectAdjusment>("SpProdJobWiserBalanceDelivery @CuttingBatchId,@CompId,@ProcessRefId,@CuttingTagId", spList).ToList();
        }

        public List<PartyWiseCuttiongProcess> GetPartyWiseCuttingDeliveryProcess(string compId,long partyId, string orderStyleRefId, string colorRefId, string componentRefId,
            bool isPrintable, bool isEmbroidery, string processCode)
        {
            SqlParameter partyuSP = new SqlParameter("@PartyId", partyId);
            SqlParameter copIdSp = new SqlParameter("@CompId", compId);
            SqlParameter processRefIdSp = new SqlParameter("@ProcessRefId", processCode);
            SqlParameter styleSp = new SqlParameter("@OrderStyleRefId", orderStyleRefId);
            SqlParameter colroSp = new SqlParameter("@ColorRefId", colorRefId??"0000");
            SqlParameter compSp = new SqlParameter("@ComponentRefId", componentRefId??"000");
            SqlParameter isPrintableSp = new SqlParameter("@isPrintable", isPrintable);
            SqlParameter isEmbroiderySp = new SqlParameter("@isEmbroidery", isEmbroidery);
            var spList = new[] { processRefIdSp, copIdSp, partyuSP, styleSp, colroSp, compSp, isPrintableSp, isEmbroiderySp };
            return Context.Database.SqlQuery<PartyWiseCuttiongProcess>("PartyWiseCuttiongProcess @ProcessRefId,@CompId,@PartyId,@OrderStyleRefId,@ColorRefId,@ComponentRefId,@isPrintable,@isEmbroidery", spList).ToList();
        }

        public List<PrintEmbProcessStatus> GetProcessStatusByStyleAndColor(string orderStyleRefId, string colorRefId, string ptype)
        {
            SqlParameter processRefIdSp = new SqlParameter("@ProcessRefId", ptype);
            SqlParameter styleSp = new SqlParameter("@OrderStyleRefId", orderStyleRefId);
            SqlParameter colroSp = new SqlParameter("@ColorRefId", colorRefId ?? "0000");
            var spList = new[] { styleSp ,colroSp, processRefIdSp };
            return Context.Database.SqlQuery<PrintEmbProcessStatus>("SpGetPrintEmbStatus @OrderStyleRefId,@ColorRefId,@ProcessRefId", spList).ToList();
        }
    }
}
