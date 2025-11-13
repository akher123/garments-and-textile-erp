using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ISewingOutPutProcessManager  
    {
       string GetNewSewingOutputProcessRefId();
       List<VwSewingOutputProcess> GetSewingOutputProcessByStyleColor(string orderStyleRefId, string colorRefId,string orderShipRefId);
       int EditSewingOutputProcess(PROD_SewingOutPutProcess model);
       int SaveSewingOutputProcess(PROD_SewingOutPutProcess model);
       int DeleteSewingOutputProcess(long sewingOutPutProcessId, string compId);
       PROD_SewingOutPutProcess GetSewintOutputProcessBySewingOutputProcessId(long sewingOutPutProcessId, string compId);
       List<VwSewingOutput> GetAllSewingOutputInfo(long sewingOutPutProcessId, string compId);
       List<VwSewingOutputProcess> GetDailySewingOut(int pageIndex, DateTime getValueOrDefault, int lineId, out int totalRecord, out long totalQty);
       List<VwSewingOutputProcess> GetDailySewingOutForReport(DateTime outputDate, int lineId);
       DataTable GetSewingWIP(DateTime outputDate, int hourId, string compId);
       bool IsSewingOutputExist(PROD_SewingOutPutProcess model);
       DataTable GetSewingWIPDetail(DateTime outputDate, string compId);
       DataTable GetHourlyProduction(DateTime productionDate, string compId);
       VwProductionForecast ProductionForecast(DateTime currentDate, string compId);
       DataTable GetSewingWIPSummary(DateTime outputDate, string compId);

       int GetTotalProductionHours(DateTime outputDate, string compId);
       VwProductionForecast ProductionForecastLastMonth(DateTime addMonths, string compId);
       List<VwSewingOutputProcess> GetDailySewingOutData(DateTime date, int lineId, string compId);
       int SaveBarcodeSewingOutputProcess
           (PROD_SewingOutPutProcess sewingOutPutProcess);
        Dictionary<string, Dictionary<string, List<string>>> GetSewingDictionaryByStyle(string orderStyleRefId);
        string GetLastSwingOutputDateTime(string compId);
    }
}
