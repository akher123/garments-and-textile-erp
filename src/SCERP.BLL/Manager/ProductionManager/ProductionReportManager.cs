using System;
using System.Collections.Generic;
using System.Data;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class ProductionReportManager:IProductionReportManager
    {
        private readonly IProductionReportRepository _productionReportRepository;
        private string _compId { get; set; }
        public ProductionReportManager(IProductionReportRepository productionReportRepository)
        {
            _productionReportRepository=productionReportRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

       

        public DataTable GetSpProdCuttiongReportSummary(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId,DateTime?cutDate)
        {
            return _productionReportRepository.GetSpProdCuttiongReportSummary(_compId,buyerRefId, orderNo, orderStyleRefId,componentRefId,cutDate);
        }

        public DataTable GetPartDesignSummary(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId)
        {
            return _productionReportRepository.GetPartDesignSummary(_compId, buyerRefId, orderNo, orderStyleRefId, componentRefId);
        }

        public DataTable GetSpProdStyleWiseTagCuttingReport(string buyerRefId, string orderNo, string orderStyleRefId,
            string componentRefId)
        {
            return _productionReportRepository.GetSpProdStyleWiseTagCuttingReport(_compId, buyerRefId, orderNo, orderStyleRefId, componentRefId);
        }


        public DataTable GetSpProdCuttiongReportDetail(string buyerRefId, string orderNo, string orderStyleRefId, string componentRefId,
            DateTime? cuttingDate)
        {
            return _productionReportRepository.GetSpProdCuttiongReportDetail(_compId, buyerRefId, orderNo, orderStyleRefId, componentRefId, cuttingDate);
        }

        public DataTable GetJobCuttingSummary(string orderStyleRefId, string componentRefId,string colorRefId)
        {
            return _productionReportRepository.GetJobCuttingSummary(_compId, orderStyleRefId, componentRefId,colorRefId);
        }
        public DataTable GetJobCuttingDetail(string orderStyleRefId, string componentRefId,string colorRefId)
        {
            return _productionReportRepository.GetJobCuttingDetail(_compId, orderStyleRefId, componentRefId,colorRefId);
        }

        public List<VwProdBundleSlip> GetBundleSlip(string cuttingBatchRefId, string compId)
        {
            return _productionReportRepository.GetBundleSlip(cuttingBatchRefId, compId);
        }

        public DataTable GetSpBundleChar(string cuttingBatchRefId, string compId)
        {
            return _productionReportRepository.GetSpBundleChar(cuttingBatchRefId, compId);
        }

        public DataTable GetSpBundle(string cuttingBatchRefId, string compId)
        {
            return _productionReportRepository.GetSpBundle(cuttingBatchRefId, compId);
        }

        public DataTable GetProcessDelivery(long processDeliveryId)
        {
            return _productionReportRepository.GetProcessDelivery(processDeliveryId, _compId);
        }

        public DataTable GetProcessReceiveDetail(string processReceiveRefId, string compId)
        {
            return _productionReportRepository.GetProcessReceiveDetail(processReceiveRefId, _compId);
        }

        public DataTable GetProcessDeliveryDatable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {
            return _productionReportRepository.GetProcessDeliveryDatable(orderStyleRefId, colorRefId, cuttingTagId,
                compId,
                processRefId);
        }

        public DataTable GetProcessReceiveDataTable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {
            return _productionReportRepository.GetProcessReceiveDataTable(orderStyleRefId, colorRefId, cuttingTagId,
                compId,
                processRefId);
        }

        public DataTable GetfactoryDataTable(string orderStyleRefId, string colorRefId, long cuttingTagId, string compId,
            string processRefId)
        {
            return _productionReportRepository.GetfactoryDataTable(orderStyleRefId, colorRefId, cuttingTagId,
                compId,
                processRefId);
        }

        public DataTable GetFactoryStyleWiseBalanceReport(string compId, long partyId, string orederStyleRefId, string processRefId)
        {
           return _productionReportRepository.GetFactoryStyleWiseBalanceReport(compId, partyId, orederStyleRefId, processRefId);
        }

        public DataTable GetMinimumSendReceive(string compId, long partyId, string orderStyleRefId, string processRefId)
        {
            return _productionReportRepository.GetMinimumSendReceive(compId, partyId, orderStyleRefId, processRefId);
        }

        public DataTable GetPrintEmbroideryBalanceSummaryt(string compId, long partyId, string orderStyleRefId)
        {
            return _productionReportRepository.GetPrintEmbroideryBalanceSummaryt(compId, partyId, orderStyleRefId);
        }

        public DataTable GetProcessDeliveryDetailReportData(string compId, string orderStyleRefId, string processRefId)
        {
            return _productionReportRepository.GetProcessDeliveryDetailReportData(compId, orderStyleRefId,processRefId);
        }

        public DataTable GetReceiveDetailReportData(string compId, string orderStyleRefId, string processRefId)
        {
            return _productionReportRepository.GetReceiveDetailReportData(compId, orderStyleRefId, processRefId);
        }

        public DataTable GetCuttBankData(string compId, string orderStyleRefId)
        {
            return _productionReportRepository.GetCuttBankData(compId, orderStyleRefId);
        }

        public DataTable GetSweingInputReport(string compId, long sewingInputProcessId)
        {
            return _productionReportRepository.GetSweingInputReport(compId, sewingInputProcessId);
        }

        public DataTable GetHourlyProductionDataTable(string compId, DateTime outputDate)
        {
            return _productionReportRepository.GetHourlyProductionDataTable(compId, outputDate);
        }

        public DataTable GetSizeLineWiseSewingOutputSummary(string compId, string orderStyleRefId, string colorRefId)
        {
            return _productionReportRepository.GetSizeLineWiseSewingOutputSummary(compId, orderStyleRefId, colorRefId);
        }

        public DataTable GetSizeLineWiseSewingOutputDetail(string compId, string orderStyleRefId, string colorRefId)
        {
            return _productionReportRepository.GetSizeLineWiseSewingOutputDetail(compId, orderStyleRefId, colorRefId);
        }

        public DataTable GetSizeLineWiseSewingInputDetail(string compId, string orderStyleRefId, string colorRefId)
        {
            return _productionReportRepository.GetSizeLineWiseSewingInputDetail(compId, orderStyleRefId, colorRefId);
        }

        public DataTable GetLineStatus(string compId, DateTime outputDate)
        {
            return _productionReportRepository.GetLineStatus(compId, outputDate);
        }

        public DataTable GetManMachineUtiliztiont(DateTime outputDate)
        {
            return _productionReportRepository.GetManMachineUtiliztiont(outputDate);
        }

        public DataTable GetDailyProductionStatus(DateTime findDate)
        {
            return _productionReportRepository.GetDailyProductionStatus(PortalContext.CurrentUser.CompId, findDate);
        }

        public DataTable GetDailyProductionStatusSummary(DateTime getValueOrDefault)
        {
            return _productionReportRepository.GetDailyProductionStatusSummary(PortalContext.CurrentUser.CompId, getValueOrDefault);
        }

        public DataTable GetMonthlyProductionStatus(int yearId, int monthId)
        {
            
           return _productionReportRepository.GetMonthlyProductionStatus(PortalContext.CurrentUser.CompId, yearId,monthId);
        }

        public DataTable GetMonthlySewingProductionStatus(int yearId, int monthId)
        {
            return _productionReportRepository.GetMonthlySewingProductionStatus(PortalContext.CurrentUser.CompId, yearId, monthId);
        }

        public DataTable GetDailyPrintEmbStatus(DateTime? filterDate)
        {
            return _productionReportRepository.GetDailyPrintEmbStatus(PortalContext.CurrentUser.CompId, filterDate);
        }

        public DataTable GetMonthlyDayWiseSewingProductionStatus(int yearId, int monthId)
        {
            return _productionReportRepository.GetMonthlyDayWiseSewingProductionStatus(PortalContext.CurrentUser.CompId, yearId, monthId);
        }

        public DataTable GetMonthlyCuttingStatus(DateTime fromDate, DateTime toDate, string compId)
        {
            return _productionReportRepository.GetMonthlyCuttingStatus(fromDate, toDate, compId);
        }

        public DataTable GetBatchDetail(long batchId, string compId)
        {
            return _productionReportRepository.GetBatchDetail(batchId,compId);
        }

        public DataTable GetKnittingProductionDetailStatus(string orderStyleRefId, string compId)
        {
            return _productionReportRepository.GetKnittingProductionDetailStatus(orderStyleRefId, compId);
        }

        public DataTable GetDyeingSpChallan(long dyeingSpChallanId)
        {
            return _productionReportRepository.GetDyeingSpChallan(dyeingSpChallanId);
        }

        public DataTable GetKnittingRollDeliveryChallan(int knittingRollIssueId)
        {
            return _productionReportRepository.GetKnittingRollDeliveryChallan(knittingRollIssueId);
        }

        public DataTable MontyPlanningVsProduction(int yearId, int monthId)
        {
            return _productionReportRepository.GetMontyPlanningVsProduction(yearId, monthId);
        }

        public DataTable GetMontyLossTimeSummaryReport(int modelYearId, int modelMonthId)
        {
            return _productionReportRepository.GetMontyLossTimeSummaryReport(modelYearId, modelMonthId);
        }

        public DataTable GetUnitWiseHourlyProduction(string compId, DateTime outputDate)
        {
            return _productionReportRepository.GetUnitWiseHourlyProduction(compId, outputDate);
        }
        public DataTable GetSewingUnitProductionForecasting(string compId, DateTime outputDate)
        {
            return _productionReportRepository.GetSewingUnitProductionForecasting(compId, outputDate);
        }

        public DataTable GetTargetVProduction(int modelYearId, int modelMonthId)
        {
            return _productionReportRepository.GetTargetVProduction(modelYearId, modelMonthId);
        }

        public DataTable GetDalilyLineWiseTargetVsProduction(string compId, DateTime outputDate)
        {
            return _productionReportRepository.GetDalilyLineWiseTargetVsProduction(compId, outputDate);
        }
        public DataTable GetKnittingRollDeliveryChallanSummary(int knittingRollIssueId)
        {
            return _productionReportRepository.GetKnittingRollDeliveryChallanSummary(knittingRollIssueId);
        }

        public DataTable GetInKnitProgramDataTable(int knittingRollIssueId)
        {
            return _productionReportRepository.GetInKnitProgramDataTable(knittingRollIssueId);
        }

        public DataTable GetMontyStyleWiseProduction(int modelYearId, int modelMonthId)
        {
            return _productionReportRepository.GetMontyStyleWiseProduction(modelYearId, modelMonthId);
        }

        public DataTable DailyProductionStatus(DateTime addDays)
        {
            return _productionReportRepository.DailyProductionStatus(addDays);
        }

        public DataTable DyeingProfitabilyAnalysis(int yearId)
        {
            DataTable dataTable= _productionReportRepository.GetDyeingProfitabilyAnalysis(yearId);
            dataTable.Columns.Add("GP", typeof(long), "(Income -DyeingCost)");
            dataTable.Columns.Add("GPKG", typeof(long), "(Income -DyeingCost)/GreyWit");
            dataTable.Columns.Add("GPPC", typeof(long), "(Income -DyeingCost)*100/Income");
            return dataTable;
        }

        public DataTable GetOrderClosingStatus(string buyerRefId, string orderNo, string orderStyleRefId)
        {
            DataTable dataTable = _productionReportRepository.GetOrderClosingStatus(buyerRefId, orderNo, orderStyleRefId);
            return dataTable;
        }

        public DataTable GetDailyProductionCapacity(DateTime filterDate)
        {
            DataTable dataTable = _productionReportRepository.GetDailyProductionCapacity(filterDate);

            return dataTable;
        }

        public DataTable GetPrintEmprocessStatus(string orderNo, string orderStyleRefId, string compId)
        {
            DataTable dataTable = _productionReportRepository.GetPrintEmprocessStatus(orderNo,orderStyleRefId,compId);

            return dataTable;
        }


        public DataTable GetKnittingRollDeliveryPartyChallan(int knittingRollIssueId)
        {
            DataTable dataTable = _productionReportRepository.GetKnittingRollDeliveryPartyChallan(knittingRollIssueId);

            return dataTable;
        }

        public DataTable GetMonthlyOtherCuttingProduction(int yearId, int monthId)
        {
            return _productionReportRepository.GetMonthlyOtherCuttingProduction(yearId, monthId);
        }
    }
}
