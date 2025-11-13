using System.Collections.Generic;
using System.Linq;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IProcessDeliveryRepository:IRepository<PROD_ProcessDelivery>
    {
       
        List<OM_Buyer> GetBuyerByPartyId(long partyId, string compId);
        IQueryable<VwProcessDelivery> GetProcessDelivery(string processRefId, string compId);
        List<SpProdJobWiseRejectAdjusment> GetJobWiserBalanceDelivery(string compId, long cuttingBatchId,long cuttingTagId, string processRefId);

        List<PartyWiseCuttiongProcess> GetPartyWiseCuttingDeliveryProcess(string compId, long partyId, string orderStyleRefId, string colorRefId, string componentRefId,
            bool isPrintable, bool isEmbroidery, string processCode);
        List<PrintEmbProcessStatus> GetProcessStatusByStyleAndColor(string orderStyleRefId, string colorRefId, string ptype);
    }
}
