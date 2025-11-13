using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IMisRepository
{
    public interface IMisDashboardRepository
    {
        DataTable GetMerchadiserWiseOrderStyleDtable();
        DataTable GetBuyerWiseOrderStyleDtable();
        DataTable GetOrderStatusSummaryDtable();
        DataTable GetSpMISReprotDashBoard(string compId);
        DataTable GetOrderStatusWithValue(string compId);
        DataTable GetBuyerOrderMaster(string buyerRefId, string orderNo, string orderStyleRefId);

        DataTable GetLineWiseSewingProduction(int currentYear, string compId);
        DataTable StyleSewingProduction(int currentYear, string currentMonth, int lineId, string compId);
        DataTable StyleSewingDetailProduction(int currentYear, string currentMonth, int lineId, string compId);
        DataTable GetLineWiseDailySewingProduction(int currentYear, string currentMonth, string compId);
        DataTable CommarcialExportImport(string compId);
        SpMisSewingProductionBoard GetSewingProductionBoard(DateTime currentDate, string compId);

        DataTable GetMontlyBuyerWiseProductionPlan();
    }
}
