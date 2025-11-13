using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IProcessReceiveManager
    {
       List<SpPodProcessReceiveBalance> GetProcessReceiveBalance(string printing, long partyId, string cuttingBatchRefId);
       string GetProcessReceiveRefNo(string prifix, string processRefId);
       Dictionary<string, List<string>> GetPrintReciveBalanceDictionary(long cuttingBatchId, long cuttingTagId, string printing);
       List<PROD_ProcessReceive> GetProcessReceiveLsitByPaging(int pageIndex, string sort, string sortdir, string searchString, long partyId, string processRefId, out int totalRecords);

       int SaveProcessReceive(PROD_ProcessReceive receive);
       int EditProcessReceive(PROD_ProcessReceive receive);
   
       PROD_ProcessReceive GetProcessReceiveById(long processReceiveId);
       Dictionary<string, Dictionary<string, PROD_ProcessReceiveDetail>> GetProcessReceiveDetailDictionary(long processReceiveId);
       Dictionary<string, List<string>> GetReceiveDictionary(long processReceiveId);
       int DeleteProcessReceiveById(long processReceiveId);
    }
}
