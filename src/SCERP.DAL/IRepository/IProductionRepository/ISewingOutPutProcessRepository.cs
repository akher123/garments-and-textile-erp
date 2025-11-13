using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository 
{
   public interface ISewingOutPutProcessRepository:IRepository<PROD_SewingOutPutProcess> 
    {
       List<VwSewingOutputProcess> GetSewingOutputProcessByStyleColor(string compId, string orderStyleRefId, string colorRefId, string orderShipRefId);
       List<VwSewingOutput> GetAllSewingOutputInfo(long sewingOutPutProcessId, string compId);
       IQueryable<VwSewingOutputProcess> GetDailySewingOut(string compId, DateTime outputDate, int lineId);
       IQueryable<VwSewingOutputProcess> GetDailySewingOutForReport(string compId, DateTime outputDate, int lineId);
       DataTable GetSewingWIP(DateTime outputDate, int hourId, string compId);
       DataTable GetSewingWIPDetail(DateTime outputDate, string compId);
       DataTable GetHourlyProduction(DateTime productionDate, string compId);
       VwProductionForecast ProductionForecast(DateTime currentDate, string compId);
       DataTable GetSewingWIPSummary(DateTime outputDate, string compId);
       VwProductionForecast ProductionForecastLastMonth(DateTime addMonths, string compId);
        string GetLastSwingOutputDateTime(string compId);
        int GetTotalProductionHours(DateTime outputDate, string compId);
    }
}
