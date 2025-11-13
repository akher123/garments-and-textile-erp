using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public  interface IMarchandisingReportManager
    {
        DataTable GetBuyerWiseOrderSummaryDataTable(DateTime? fromDate, DateTime? toDate, string buyerRefId);

        DataTable GetConfirmedOrderStatus();
        DataTable GetDetailOrderStatus();
        DataTable GetProductionStatus(DateTime? fromDate, DateTime? toDate);
        DataTable GetShipmentStatus(string seasonRefId, string merchandiserId, string buyerRefId);

        DataTable GetPandingConsumptionDataTable(string merchandiserId);
        DataTable GetPoSheet(long purchaseOrderId);
        DataTable GetFabricWorkOrderSheet(string orderStyleRefId);
        DataTable GetFabricWorkOrderDetailSheet(string orderStyleRefId);
        DataTable GetBulkFabricOrderSheet(int fabricOrderId);
        DataTable GetRunningOrderStatus();
        DataTable GetSeasonWiseOrderSummary(DateTime?fromDate,DateTime?todate);
        int SendMailExecut();
        DataTable GetYarnWorkOrderSheet(long purchaseOrderId);
        DataTable GetBulkYarnBooking(long bulkBookingId);
        DataTable GetOrderShipmentSummary(DateTime? fromDate, DateTime? toDate, Guid? userId);
        DataTable GetRunningOrderOrderStatus(string compId);
        DataTable GetPreCostSheet(string orderStyleRefId,string compId);
        DataTable GetStyleWiseProduction(string orderStyleRefId, string compId);
        DataTable GetCollarCuffBulkFabricOrderSheet(int fabricOrderId);
        DataTable GetShipmentAlert(string buyerRefId, string orderNo, string orderStyleRefId, string compId);
    }
}
