using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IMisManager
{
    public interface IMisDashboardManager
    {
        DataTable GetMerchadiserWiseOrderStyleDtable();
        DataTable GetBuyerWiseOrderStyleDtable();
        DataTable GetOrderStatusSummaryDtable();
        DataTable GetSpMISReprotDashBoard();
        DataTable GetOrderStatusWithValue();

        DataTable RunningOrderSummary(string month, string buyer, string compId);
        DataTable MachineWiseKnitting(DateTime? fromDate, string searchString, string compId);
        DataTable GetMonthlyOrderStatus(string compId);
        DataTable GetSpBuyerOrderMasterDashBoard(string BuyerRefId,string OrderNo,string OrderStyleRefId);
        DataTable GetLineWiseSewingProduction   (int currentYear, string compId);
        DataTable StyleSewingProduction(int currentYear, string currentMonth, int lineId, string compId);
        DataTable StyleSewingDetailProduction(int currentYear, string currentMonth, int lineId, string compId);
        DataTable GetLineWiseDailySewingProduction(int currentYear, string currentMonth, string compId);
        DataTable CommarcialExportImport();
        SpMisSewingProductionBoard GetSewingProductionBoard(DateTime currentDate, string compId);
        DataTable GetMontlyBuyerWiseProductionPlan();
        DataTable MontlySewingPlanDetail(string month, string buyer, string compId);
        List<dynamic> GetDailyTargetAchivemnet(DateTime currentDate, string compId);
        List<dynamic> GetHourlyTargetAchivemnet(DateTime currentDate, string compId);
        DataTable GetTnaSweingPandingStatus();
        DataTable GetTnaSweingUpcomingStatus();
        DataTable GetShipmentRatio(string orderStyleRefId);
    }
}
