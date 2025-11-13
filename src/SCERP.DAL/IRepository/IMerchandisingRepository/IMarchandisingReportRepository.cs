using System;
using System.Data;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IMarchandisingReportRepository:IRepository<Object>
   {
 
       DataTable GetBuyerWiseOrderSummaryDataTable(DateTime? fromDate, DateTime? toDate, string buyerRefId, string companyId);
       DataTable GetConfirmedOrderStatus(string companyId);
       DataTable GetDetailOrderStatus(string companyId);
       DataTable GetProductionStatus(string companyId, DateTime? fromDate, DateTime? toDate);
       DataTable GetShipmentStatus(string seasonRefId, string merchandiserId, string buyerRefId, string companyId);

       DataTable GetPandingConsumptionDataTable(string merchandiserId, string companyId);
       DataTable GetPoSheet(long purchaseOrderId, string companyId);
       DataTable GetFabricWorkOrderSheet(string orderStyleRefId, string companyId);
       DataTable GetFabricWorkOrderDetailSheet(string orderStyleRefId, string companyId);
       DataTable GetBulkFabricOrderSheet(int fabricOrderId, string companyId);
       DataTable GetRunningOrderStatus(string companyId);
       DataTable GetSeasonWiseOrderSummary(DateTime? fromDate, DateTime? todate,string companyId);
       int SendMailExecut();
       DataTable GetYarnWorkOrderSheet(long purchaseOrderId, string companyId);
       DataTable GetBulkYarnBooking(long purchaseOrderId, string companyId);
       DataTable GetOrderShipmentSummary(DateTime? fromDate, DateTime? toDate, Guid? userId, string companyId);
       DataTable GetRunningOrderOrderStatus(string compId);
       DataTable GetPreCostSheet(string orderStyleRefId, string compId);
       DataTable GetStyleWiseProduction(string orderStyleRefId, string compId);
       DataTable GetCollarCuffBulkFabricOrderSheet(int fabricOrderId, string companyId);

       DataTable GetShipmentAlert(string buyerRefId, string orderNo, string orderStyleRefId, string compId);
       string GetInWord(decimal sumTotal);
   }
}
