using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IProcessDeliveryManager
    {
   
        int SaveProcessDelivery(PROD_ProcessDelivery delivery);
        string GetPrintingDeliveryRefNo();
        Dictionary<string, Dictionary<string, PROD_ProcessDeliveryDetail>> GetProcessDeliveryDictionary(long processDeliveryId);
        PROD_ProcessDelivery GetProcessDeliveryById(long processDeliveryId);
        Dictionary<string, List<string>> GetSizeNameDictionry(long processDeliveryId);
        int EditProcessDelivery(PROD_ProcessDelivery delivery);
        int DeleteProcessDelivery(long processDeliveryId);
        List<VwProcessDelivery> GetProcessDelivery(int pageIndex, string processRefId, long partyId, string searchString,string orderStyleRefId, out int totalRecords);
        string GetEmbroideryDeliveryRefNo();
        List<OM_Buyer> GetBuyerByPartyId(long partyId);

        
        Dictionary<string, List<string>> GetJobWiserBalanceDelivery(long cuttingBatchId, long cuttingTagId, string embroidary);
        List<PartyWiseCuttiongProcess> GetPartyWiseCuttingDeliveryProcess
            (long partyId, string orderStyleRefId, string colorRefId, string componentRefId, bool isPrintable, bool isEmbroidery, string embroidary);
        List<VwProcessDelivery> ProcessDeliverySummaryReport(string processRetId, string serachString, long partyId);
        Dictionary<string, Dictionary<string, List<string>>> GetProcessStatusByStyle(string orderStyleRefId, string v);
    }
}
