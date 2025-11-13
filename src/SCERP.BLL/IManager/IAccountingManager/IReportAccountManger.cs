using System.Data;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.AccountingModel;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IReportAccountManger
    {
        System.Data.DataTable GetReport();

        List<ChartOfAccountsReportModel> GetChartOfAccountsReportData();

        List<VoucherModel> GetVoucherInfo(long id);

        List<VoucherModel> GetVoucherMultiCurrencyInfo(long id);

        List<GeneralLedgerDetailModel> GetGeneralLedgerDetail(string accountCode, int? sectorId, int? costCentreId, int currencyId, DateTime? fromDate, DateTime? toDate);

        System.Data.DataTable GetGeneralLedger(Acc_ReportViewModel iObj);

        System.Data.DataTable GetTrialBalance(Acc_ReportViewModel iObj);

        DataTable GetTrialBalanceCostCentre(Acc_ReportViewModel iObj);

        System.Data.DataTable GetCashFlowData(Acc_ReportViewModel model);

        DataTable GetTrialbalanceNew(Acc_ReportViewModel iObj);

        DataTable GetTrialbalanceCostCentre(Acc_ReportViewModel iObj);

        string GetVoucherNoByRefNo(long refNo);

        long GetVoucherIdbyVoucherNo(int? voucherNo);

        List<Acc_IncomeStatementMfgModel> GetIncomeStatementMfg(int companySectoryId, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetVoucherStatement(string voucherType, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetVoucherSummary(string voucherType, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetReceivePaymentData(int sectorId, string controlCode1, string controlCode2, string fromDate, string toDate);

        int? GetActiveCurrencyByVoucherMasterId(long id);

        GeneralLedgerDetailModel GetOpeningBalanceByGlId(string sectorId, DateTime fromDate, DateTime toDate, string accountCode);

        Acc_FinancialPeriod GetFinancialPeriod(string financialPeriodId);

        Acc_FinancialPeriod GetActiveFinancialPeriod();

        int GetFinancialPeriodId(DateTime fromDate, DateTime toDate);

        DataTable GetGetCashBook(DateTime getValueOrDefault, DateTime dateTime, int glId);

        DataTable GetControlTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetGLTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetSubGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetControlLedger(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode);

        DataTable GetBalanceSheetNote(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode);

        DataTable GetAgingData(int SectorId, DateTime fromDate);
    }
}