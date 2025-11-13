using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using SCERP.Model;
using SCERP.Model.Custom;
using SCERP.Model.AccountingModel;
using SCERP.BLL.Manager.CommonManager;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class ReportAccountManger : IReportAccountManger
    {
        private IReportAccountRepository reportAccountRepository = null;

        public ReportAccountManger(SCERPDBContext context)
        {
            this.reportAccountRepository = new ReportAccountRepository(context);
        }

        public DataTable GetReport()
        {
            return reportAccountRepository.GetReport();
        }

        public List<ChartOfAccountsReportModel> GetChartOfAccountsReportData()
        {
            return reportAccountRepository.GetChartOfAccountsReportData();
        }

        public List<VoucherModel> GetVoucherInfo(long id)
        {
            return reportAccountRepository.GetVoucherInfo(id);
        }

        public long GetVoucherIdbyVoucherNo(int? voucherNo)
        {
            return reportAccountRepository.GetVoucherIdbyVoucherNo(voucherNo);
        }

        public List<VoucherModel> GetVoucherMultiCurrencyInfo(long id)
        {
            return reportAccountRepository.GetVoucherMultiCurrencyInfo(id);
        }

        public List<Acc_IncomeStatementMfgModel> GetIncomeStatementMfg(int companySectoryId, DateTime? fromDate, DateTime? toDate)
        {
            return reportAccountRepository.GetIncomeStatementMfg(companySectoryId, fromDate, toDate);
        }

        public List<VoucherModel> GetVoucherStatement(string voucherType, DateTime? fromDate, DateTime? toDate)
        {
            return reportAccountRepository.GetVoucherStatement(voucherType, fromDate, toDate);
        }

        public List<VoucherModel> GetVoucherSummary(string voucherType, DateTime? fromDate, DateTime? toDate)
        {
            return reportAccountRepository.GetVoucherSummary(voucherType, fromDate, toDate);
        }

        public List<GeneralLedgerDetailModel> GetGeneralLedgerDetail(string accountCode, int? sectorId, int? costCentreId, int currencyId, DateTime? fromDate, DateTime? toDate)
        {
            return reportAccountRepository.GetGeneralLedgerDetail(accountCode, sectorId, costCentreId, currencyId, fromDate, toDate);
        }

        public DataTable GetGeneralLedger(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetGeneralLedger(iObj);
        }

        public DataTable GetTrialBalance(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetTrialBalance(iObj);
        }

        public DataTable GetTrialBalanceCostCentre(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetTrialBalanceCostCentre(iObj);
        }

        public List<VoucherModel> GetReceivePaymentData(int sectorId, string controlCode1, string controlCode2, string fromDate, string toDate)
        {
            return reportAccountRepository.GetReceivePaymentData(sectorId, controlCode1, controlCode2, fromDate, toDate);
        }

        public DataTable GetTrialbalanceNew(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetTrialbalanceNew(iObj);
        }

        public DataTable GetTrialbalanceCostCentre(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetTrialbalanceCostCentre(iObj);
        }

        public DataTable GetCashFlowData(Acc_ReportViewModel iObj)
        {
            return reportAccountRepository.GetCashFlowData(iObj);
        }

        public string GetVoucherNoByRefNo(long refNo)
        {
            return reportAccountRepository.GetVoucherNoByRefNo(refNo);
        }

        public int? GetActiveCurrencyByVoucherMasterId(long id)
        {
            return reportAccountRepository.GetActiveCurrencyByVoucherMasterId(id);
        }

        public GeneralLedgerDetailModel GetOpeningBalanceByGlId(string sectorId, DateTime fromDate, DateTime toDate, string accountCode)
        {
            return reportAccountRepository.GetOpeningBalanceByGlId(sectorId, fromDate, toDate, accountCode);
        }

        public Acc_FinancialPeriod GetFinancialPeriod(string financialPeriodId)
        {
            return reportAccountRepository.GetFinancialPeriod(financialPeriodId);
        }

        public Acc_FinancialPeriod GetActiveFinancialPeriod()
        {
            return reportAccountRepository.GetActiveFinancialPeriod();
        }

        public int GetFinancialPeriodId(DateTime fromDate, DateTime toDate)
        {
            return reportAccountRepository.GetFinancialPeriodId(fromDate, toDate);
        }

        public DataTable GetGetCashBook(DateTime fromDate, DateTime toDate, int glId)
        {
            return reportAccountRepository.GetGetCashBook(fromDate, toDate, glId);
        }

        public DataTable GetControlTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            return reportAccountRepository.GetControlTrialBalance(SectorId, fromDate, toDate, FpId);
        }

        public DataTable GetGLTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            return reportAccountRepository.GetGLTrialBalance(SectorId, fromDate, toDate, FpId);
        }

        public DataTable GetSubGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            return reportAccountRepository.GetSubGroupTrialBalance(SectorId, fromDate, toDate, FpId);
        }

        public DataTable GetGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            return reportAccountRepository.GetGroupTrialBalance(SectorId, fromDate, toDate, FpId);
        }
        public DataTable GetControlLedger(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode)
        {
            return reportAccountRepository.GetControlLedger(SectorId, fromDate, toDate, FpId, ControlCode);
        }
        public DataTable GetBalanceSheetNote(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode)
        {
            return reportAccountRepository.GetBalanceSheetNote(SectorId, fromDate, toDate, FpId, ControlCode);
        }
        public DataTable GetAgingData(int SectorId, DateTime fromDate)
        {
            return reportAccountRepository.GetAgingData(SectorId, fromDate);
        }
    }
}