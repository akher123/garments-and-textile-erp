using System;
using System.Collections.Generic;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IFinishingProcessManager
    {
        string GetNewFinishingProcessRefId(int finishType, string prifix);
        List<VwFinishingProcessDetail> GetFinishingProcessDetailByStyleColor(string orderStyleRefId, string colorRefId, long finishType);
        List<VwFinishingProcess> GetSewingInputProcessByStyleColor(string orderStyleRefId, string colorRefId, int finishType);
        int EditFinishingProcess(PROD_FinishingProcess finishingProcess);
        int SaveFinishingProcess(PROD_FinishingProcess finishingProcess);
  
        PROD_FinishingProcess GeFabricProcessById(long finishingProcessId);
        List<VwFinishingProcessDetail> GetPolyFinishingProcessDetailByStyleColor
            (string orderStyleRefId, string colorRefId, long finishingProcessId);

        int DeleteFinishingProcess(long finishingProcessId);
        List<VwFinishingProcess> GetFinishingProcess(DateTime? inputDate, int finishType);
        Dictionary<string, Dictionary<string, List<string>>> GetFinishingDictionaryByStyle(string orderStyleRefId);
    }
}
