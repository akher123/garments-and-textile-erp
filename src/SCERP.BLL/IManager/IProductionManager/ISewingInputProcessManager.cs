using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;


namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ISewingInputProcessManager
    { 
       string GetNewSewingInputProcessRefId();
       int SaveSewingInputProcess(PROD_SewingInputProcess model);
       List<VwSewingInputProcess> GetSewingInputProcessByStyleColor(string orderStyleRefId, string colorRefId,string orderShipRefId);
       List<VwSewingInputProcessDetail> GetAllSewingInputInfo(long sewingInputProcessId);
       PROD_SewingInputProcess GetSewintInputProcessBySewingInputProcessId(long sewingInputProcessId, string compId);
       int EditSewingInputProcess(PROD_SewingInputProcess model);
       int DeleteSewingInputProcess(long sewingInputProcessId, string compId);
       List<VwSewingOutput> GetVwSewingInput(string orderStyleRefId, string colorRefId, string orderShipRefId);
       List<VwSewingInputProcess> GetSewingInputByPaging(DateTime? inputDate, int lineId, int pageIndex, out int totalRecords, out int totalInput);
       bool IsSewingInputExist(PROD_SewingInputProcess model);
       int SaveSweingInBarcode(PROD_SewingInputProcess sewingInputProcess);
       List<VwSewingInputProcess> DailySweingInPut(DateTime date,int lineId, string compId);
       PROD_SewingInputProcess GetInputByBundleId(string bundleId);
        int SweingAccessoriesIssueLock(long sewingInputProcessId);
    }
}
