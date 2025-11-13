using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IProductionReportManager 
    {
   
        DataTable GetSpProdCuttiongReportSummary(string buyerRefId,string orderNo, string orderStyleRefId, string componentRefId,DateTime? cuttDate);
        DataTable GetPartDesignSummary(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId);
        DataTable GetSpProdStyleWiseTagCuttingReport(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId);

        DataTable GetSpProdCuttiongReportDetail(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId, DateTime? cuttingDate);

        DataTable GetJobCuttingSummary(string orderStyleRefId, string componentRefId, string colorRefId);
        DataTable GetJobCuttingDetail(string orderStyleRefId, string componentRefId,string colorRefId);
        List<VwProdBundleSlip> GetBundleSlip(string cuttingBatchRefId, string compId);
        DataTable GetSpBundleChar(string cuttingBatchRefId, string compId);
        DataTable GetSpBundle(string cuttingBatchRefId, string compId);
        DataTable GetProcessDelivery(long processDeliveryId);
        DataTable GetProcessReceiveDetail(string processReceiveRefId, string compId);

        DataTable GetProcessDeliveryDatable
            (string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);

        DataTable GetProcessReceiveDataTable
            (string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);

        DataTable GetfactoryDataTable
            (string orderStyleRefId, string colorRefId, long cuttingTagId, string compId, string processRefId);

        DataTable GetFactoryStyleWiseBalanceReport(string compId, long partyId, string orederStyleRefId, string processRefId);
        DataTable GetMinimumSendReceive(string compId, long partyId, string orderStyleRefId, string processRefId);

        DataTable GetPrintEmbroideryBalanceSummaryt(string compId, long partyId, string orderStyleRefId);

        DataTable GetProcessDeliveryDetailReportData(string compId, string orderStyleRefId, string processRefId);
        DataTable GetReceiveDetailReportData(string compId, string orderStyleRefId, string printing);
        DataTable GetCuttBankData(string compId, string orderStyleRefId);
        DataTable GetSweingInputReport(string compId, long sewingInputProcessId);
        DataTable GetHourlyProductionDataTable(string compId, DateTime outputDate);
        DataTable GetSizeLineWiseSewingOutputSummary(string compId, string orderStyleRefId, string colorRefId);
        DataTable GetSizeLineWiseSewingOutputDetail(string compId, string orderStyleRefId, string colorRefId);
        DataTable GetSizeLineWiseSewingInputDetail
            (string compId, string orderStyleRefId, string colorRefId);

        DataTable GetLineStatus(string compId, DateTime outputDate);
        DataTable GetManMachineUtiliztiont(DateTime outputDate);
        DataTable GetDailyProductionStatus(DateTime findDate);
        DataTable GetDailyProductionStatusSummary(DateTime getValueOrDefault);
        DataTable GetMonthlyProductionStatus(int yearId, int monthId);
        DataTable GetMonthlySewingProductionStatus(int yearId, int monthId);
        DataTable GetDailyPrintEmbStatus(DateTime? filterDate);
        DataTable GetMonthlyDayWiseSewingProductionStatus(int yearId, int monthId);
        DataTable GetMonthlyCuttingStatus(DateTime fromDate, DateTime toDate, string compId);
        DataTable GetBatchDetail(long batchId, string compId);
        DataTable GetKnittingProductionDetailStatus(string orderStyleRefId, string compId);
        DataTable GetDyeingSpChallan(long dyeingSpChallanId);
        DataTable GetKnittingRollDeliveryChallan(int knittingRollIssueId);
        DataTable MontyPlanningVsProduction(int yearId, int monthId);
        DataTable GetMontyLossTimeSummaryReport(int modelYearId, int modelMonthId);
        DataTable GetUnitWiseHourlyProduction(string compId, DateTime outputDate);
        DataTable GetSewingUnitProductionForecasting(string compId, DateTime outputDate);
        DataTable GetTargetVProduction(int modelYearId, int modelMonthId);
        DataTable GetDalilyLineWiseTargetVsProduction(string compId, DateTime outputDate);
        DataTable GetKnittingRollDeliveryChallanSummary(int knittingRollIssueId);
        DataTable GetInKnitProgramDataTable(int knittingRollIssueId);
        DataTable GetMontyStyleWiseProduction(int modelYearId, int modelMonthId);
        DataTable DailyProductionStatus(DateTime addDays);
        DataTable DyeingProfitabilyAnalysis(int yearId);
        DataTable GetOrderClosingStatus(string buyerRefId, string orderNo, string orderStyleRefId);
        DataTable GetDailyProductionCapacity(DateTime filterDate);
        DataTable GetPrintEmprocessStatus(string orderNo, string orderStyleRefId, string compId);
        DataTable GetKnittingRollDeliveryPartyChallan(int knittingRollIssueId);
        DataTable GetMonthlyOtherCuttingProduction(int yearId, int monthId);
    }
}
