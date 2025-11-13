using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IProductionReportRepository
    {

        DataTable GetSpProdCuttiongReportSummary
            (string compId,string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId,DateTime? cutDate);


        DataTable GetSpProdStyleWiseTagCuttingReport(string compId, string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId);


        DataTable GetSpProdCuttiongReportDetail
            (string compId, string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId, object cutDate);

        DataTable GetJobCuttingSummary
            (string compId, string orderStyleRefId, string componentRefId, string colorRefId);

        DataTable GetJobCuttingDetail(string compId, string orderStyleRefId, string componentRefId, string colorRefId);
        DataTable GetPartDesignSummary(string compId, string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId);
        List<VwProdBundleSlip> GetBundleSlip(string cuttingBatchRefId, string compId);
        DataTable GetSpBundleChar(string cuttingBatchRefId, string compId);
        DataTable GetSpBundle(string cuttingBatchRefId, string compId);
        DataTable GetProcessDelivery(long processDeliveryId, string compId);
        DataTable GetProcessReceiveDetail(string processReceiveRefId, string compId);
        DataTable GetfactoryDataTable
            (string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);

        DataTable GetProcessReceiveDataTable
            (string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);

        DataTable GetProcessDeliveryDatable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);
        DataTable GetFactoryStyleWiseBalanceReport(string compId, long partyId, string orederStyleRefId, string processRefId);
        DataTable GetMinimumSendReceive
            (string compId, long partyId, string orderStyleRefId, string processRefId);

        DataTable GetPrintEmbroideryBalanceSummaryt(string compId, long partyId, string orderStyleRefId);
        DataTable GetProcessDeliveryDetailReportData(string compId, string orderStyleRefId, string processRefId);
        DataTable GetReceiveDetailReportData(string compId, string orderStyleRefId, string processRefId);
        DataTable GetCuttBankData(string compId, string orderStyleRefId);
        DataTable GetSweingInputReport(string compId, long sewingInputProcessId);
        DataTable GetHourlyProductionDataTable(string compId, DateTime outputDate);
        DataTable GetSizeLineWiseSewingOutputSummary(string compId, string orderStyleRefId, string colorRefId);
        DataTable GetSizeLineWiseSewingOutputDetail(string compId, string orderStyleRefId, string colorRefId);
        DataTable GetSizeLineWiseSewingInputDetail(string compId, string orderStyleRefId, string colorRefId);

        DataTable GetLineStatus(string compId, DateTime outputDate);
        DataTable GetManMachineUtiliztiont(DateTime outputDate);
        DataTable GetDailyProductionStatus(string compId, DateTime findDate);
        DataTable GetDailyProductionStatusSummary(string compId, DateTime getValueOrDefault);
        DataTable GetMonthlyProductionStatus(string compId, int yearId, int monthId);
        DataTable GetMonthlySewingProductionStatus(string compId, int yearId, int monthId);
        DataTable GetDailyPrintEmbStatus(string compId, DateTime? filterDate);
        DataTable GetMonthlyDayWiseSewingProductionStatus(string compId, int yearId, int monthId);
        DataTable GetMonthlyCuttingStatus(DateTime fromDate, DateTime toDate, string compId);
        DataTable GetBatchDetail(long batchId, string compId);
        DataTable GetKnittingProductionDetailStatus(string orderStyleRefId, string compId);
        DataTable GetDyeingSpChallan(long dyeingSpChallanId);
        DataTable GetKnittingRollDeliveryChallan(int knittingRollIssueId);
        DataTable GetMontyPlanningVsProduction(int yearId, int monthId);
        DataTable GetMontyLossTimeSummaryReport(int modelYearId, int modelMonthId);
        DataTable GetUnitWiseHourlyProduction(string compId, DateTime outputDate);
        DataTable GetSewingUnitProductionForecasting(string compId, DateTime outputDate);
        DataTable GetTargetVProduction(int modelYearId, int modelMonthId);
        DataTable GetDalilyLineWiseTargetVsProduction(string compId, DateTime outputDate);
        DataTable GetKnittingRollDeliveryChallanSummary(int knittingRollIssueId);
        DataTable GetInKnitProgramDataTable(int knittingRollIssueId);
        DataTable GetMontyStyleWiseProduction(int modelYearId, int modelMonthId);
        DataTable DailyProductionStatus(DateTime addDays);
        DataTable GetDyeingProfitabilyAnalysis(int yearId);
        DataTable GetOrderClosingStatus(string buyerRefId,string orderNo,string orderStyleRefId);
        DataTable GetDailyProductionCapacity(DateTime filterDate);
        DataTable GetPrintEmprocessStatus(string orderNo, string orderStyleRefId, string compId);
        DataTable GetKnittingRollDeliveryPartyChallan(int knittingRollIssueId);
        DataTable GetMonthlyOtherCuttingProduction(int yearId, int monthId);
    }
}
