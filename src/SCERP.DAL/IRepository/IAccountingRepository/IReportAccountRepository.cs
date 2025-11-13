using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Custom;
using SCERP.Model.AccountingModel;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IReportAccountRepository
    {
        DataTable GetReport();

        List<ChartOfAccountsReportModel> GetChartOfAccountsReportData();

        List<VoucherModel> GetVoucherInfo(long id);

        List<VoucherModel> GetVoucherMultiCurrencyInfo(long id);

        List<GeneralLedgerDetailModel> GetGeneralLedgerDetail(string accountCode, int? sectorId, int? costCentreId, int currencyId, DateTime? fromDate, DateTime? toDate);

        DataTable GetGeneralLedger(Acc_ReportViewModel iObj);

        DataTable GetTrialBalance(Acc_ReportViewModel iObj);

        DataTable GetTrialBalanceCostCentre(Acc_ReportViewModel iObj);

        DataTable GetCashFlowData(Acc_ReportViewModel iObj);

        DataTable GetTrialbalanceNew(Acc_ReportViewModel iObj);

        DataTable GetTrialbalanceCostCentre(Acc_ReportViewModel iObj);

        string GetVoucherNoByRefNo(long refNo);

        List<Acc_IncomeStatementMfgModel> GetIncomeStatementMfg(int companySectoryId, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetVoucherStatement(string voucherType, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetVoucherSummary(string voucherType, DateTime? fromDate, DateTime? toDate);

        List<VoucherModel> GetReceivePaymentData(int sectorId, string controlCode1, string controlCode2, string fromDate, string toDate);

        int? GetActiveCurrencyByVoucherMasterId(long id);

        long GetVoucherIdbyVoucherNo(int? voucherNo);

        GeneralLedgerDetailModel GetOpeningBalanceByGlId(string sectorId, DateTime fromDate, DateTime toDate, string accountCode);

        Acc_FinancialPeriod GetFinancialPeriod(string financialPeriodId);

        Acc_FinancialPeriod GetActiveFinancialPeriod();

        int GetFinancialPeriodId(DateTime fromDate, DateTime toDate);

        DataTable GetGetCashBook(DateTime fromDate, DateTime toDate, int glId);

        DataTable GetControlTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetGLTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetSubGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId);

        DataTable GetControlLedger(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode);

        DataTable GetBalanceSheetNote(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode);

        DataTable GetAgingData(int SectorId, DateTime fromDate);
    }
}