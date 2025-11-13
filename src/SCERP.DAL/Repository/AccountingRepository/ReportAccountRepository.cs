using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using SCERP.Model.Custom;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.AccountingRepository
{
    public class ReportAccountRepository : IReportAccountRepository
    {
        private readonly SCERPDBContext _context;

        private SqlConnection _connection;

        public ReportAccountRepository(SCERPDBContext context)
        {
            this._context = context;
            _connection = (SqlConnection) _context.Database.Connection;
        }

        public List<VoucherModel> GetVoucherInfo(long id)
        {

            var attdModel = new List<VoucherModel>();

            try
            {
                var spVoucherInfo = _context.SPVoucherReport(id);

                foreach (var voucherInfo in spVoucherInfo)
                {
                    var voucherModel = new VoucherModel
                    {
                        Id = voucherInfo.Id,
                        VoucherType = voucherInfo.VoucherType,
                        VoucherNo = voucherInfo.VoucherNo,
                        VoucherDate = voucherInfo.VoucherDate,
                        CheckNo = voucherInfo.CheckNo,
                        VoucherRefNo = voucherInfo.VoucherRefNo,
                        CheckDate = voucherInfo.CheckDate,
                        Particulars = voucherInfo.Particulars,
                        GLID = voucherInfo.GLID,
                        DetailParticulars = voucherInfo.DetailParticulars,
                        Debit = voucherInfo.Debit,
                        Credit = voucherInfo.Credit,
                        TotalAmountInWord = voucherInfo.TotalAmountInWord,
                        ControlCode = voucherInfo.ControlCode,
                        AccountCode = voucherInfo.AccountCode,
                        AccountName = voucherInfo.AccountName,
                        SectorCode = voucherInfo.SectorCode,
                        SectorName = voucherInfo.SectorName,
                        CostCentreCode = voucherInfo.CostCentreCode,
                        CostCentreName = voucherInfo.CostCentreName,
                        PeriodName = voucherInfo.PeriodName,
                        PeriodStartDate = voucherInfo.PeriodStartDate,
                        PeriodEndDate = voucherInfo.PeriodEndDate,
                        Address = voucherInfo.Address
                    };

                    attdModel.Add(voucherModel);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return attdModel;
        }

        public List<VoucherModel> GetVoucherMultiCurrencyInfo(long id)
        {
            List<VoucherModel> voucher;

            try
            {
                voucher = _context.Database.SqlQuery<VoucherModel>("SPVoucherMultiCurrencyReport @Id", new SqlParameter("Id", id)).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return voucher;
        }

        public List<VoucherModel> GetVoucherStatement(string voucherType, DateTime? fromDate, DateTime? toDate)
        {
            var voucherModel = new List<VoucherModel>();

            try
            {
                voucherModel = _context.Database.SqlQuery<VoucherModel>("SPVoucherStatementReport @VoucherType, @FromDate, @ToDate", new SqlParameter("VoucherType", voucherType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return voucherModel;
        }

        public List<Acc_IncomeStatementMfgModel> GetIncomeStatementMfg(int companySectoryId, DateTime? fromDate, DateTime? toDate)
        {
            var incomeStatement = new List<Acc_IncomeStatementMfgModel>();

            try
            {
                incomeStatement = _context.Database.SqlQuery<Acc_IncomeStatementMfgModel>("SPIncomeStatementMfgReport @CompanySectorId, @FromDate, @ToDate", new SqlParameter("CompanySectorId", companySectoryId), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();

                foreach (var t in incomeStatement)
                {
                    t.Particulars = t.Particulars.Replace("\r\n", "");
                }
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return incomeStatement;
        }

        public List<VoucherModel> GetVoucherSummary(string voucherType, DateTime? fromDate, DateTime? toDate)
        {
            var statement = new List<VoucherModel>();

            try
            {
                statement = _context.Database.SqlQuery<VoucherModel>("SPVoucherSummaryReport @VoucherType, @FromDate, @ToDate", new SqlParameter("VoucherType", voucherType), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();

                var hiddenList = from p in _context.Acc_GLAccounts_Hiddens
                                 where p.IsActive == true
                                 select p;

                foreach (var qs in hiddenList)
                {
                    statement = statement.Where(x => x.AccountCode != qs.AccountCode).ToList();
                }
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return statement;
        }

        public List<VoucherModel> GetReceivePaymentData(int sectorId, string controlCode1, string controlCode2, string fromDate, string toDate)
        {
            var receivePayment = new List<VoucherModel>();

            try
            {
                receivePayment = _context.Database.SqlQuery<VoucherModel>("SPGetReceivePaymentDataReport @SectorId, @ControlCode1,@ControlCode2, @FromDate, @ToDate", new SqlParameter("SectorId", sectorId), new SqlParameter("ControlCode1", controlCode1), new SqlParameter("ControlCode2", controlCode2), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return receivePayment;
        }

        public List<GeneralLedgerDetailModel> GetGeneralLedgerDetail(string accountCode, int? sectorId, int? costCentreId, int currencyId, DateTime? fromDate, DateTime? toDate)
        {

            decimal? balance;
            decimal? currencyValue = 1;
            decimal? openingBalance = (decimal?) 0.0;

            costCentreId = 1;

            var glModel = new List<GeneralLedgerDetailModel>();
            var accountCodeDecimal = Convert.ToDecimal(accountCode);

            if (currencyId == 1)
            {
                openingBalance = (from p in _context.Acc_VoucherDetail
                    join q in _context.Acc_GLAccounts on p.GLID equals q.Id
                    join r in _context.Acc_VoucherMaster on p.RefId equals r.Id
                    where q.AccountCode == accountCodeDecimal && r.VoucherDate < fromDate && r.SectorId == sectorId
                    select p.Debit - p.Credit).Sum();
            }

            if (currencyId == 2)
            {
                openingBalance = (from p in _context.Acc_VoucherDetail
                    join q in _context.Acc_GLAccounts on p.GLID equals q.Id
                    join r in _context.Acc_VoucherMaster on p.RefId equals r.Id
                    where q.AccountCode == accountCodeDecimal && r.VoucherDate < fromDate && r.SectorId == sectorId
                    select p.Debit/p.SecendCurValue - p.Credit/p.SecendCurValue).Sum();
            }

            if (currencyId == 3)
            {
                openingBalance = (from p in _context.Acc_VoucherDetail
                    join q in _context.Acc_GLAccounts on p.GLID equals q.Id
                    join r in _context.Acc_VoucherMaster on p.RefId equals r.Id
                    where q.AccountCode == accountCodeDecimal && r.VoucherDate < fromDate && r.SectorId == sectorId
                    select p.Debit/p.ThirdCurValue - p.Credit/p.ThirdCurValue).Sum();
            }

            balance = (decimal?) Math.Round((double) (openingBalance ?? 0), 2);

            try
            {
                var spgeneralLedger = new List<GeneralLedgerDetailModel>();

                spgeneralLedger = _context.Database.SqlQuery<GeneralLedgerDetailModel>("SPGeneralLedgerDetailReport @AccountCode, @SectorId, @CostCentreId, @FromDate, @ToDate", new SqlParameter("AccountCode", accountCode), new SqlParameter("SectorId", sectorId), new SqlParameter("CostCentreId", costCentreId), new SqlParameter("FromDate", fromDate), new SqlParameter("ToDate", toDate)).ToList();

                foreach (var generalLedger in spgeneralLedger)
                {
                    balance = balance + generalLedger.Credit - generalLedger.Debit;

                    if (currencyId == 1)
                        currencyValue = generalLedger.FirstCurValue;
                    else if (currencyId == 2)
                        currencyValue = generalLedger.SecendCurValue;
                    else if (currencyId == 3)
                        currencyValue = generalLedger.ThirdCurValue;

                    var GeneralLedgerDetailModel = new GeneralLedgerDetailModel
                    {
                        SectorName = generalLedger.SectorName,
                        CostCentreName = generalLedger.CostCentreName,
                        VoucherNo = generalLedger.VoucherNo,
                        VoucherRefNo = generalLedger.VoucherRefNo,
                        GLNameSearch = generalLedger.GLNameSearch,
                        GLName = generalLedger.GLName,
                        Debit = (decimal?) Math.Round((double) (generalLedger.Debit/currencyValue), 2),
                        Credit = (decimal?) Math.Round((double) (generalLedger.Credit/currencyValue), 2),
                        VoucherDate = generalLedger.VoucherDate,
                        FromDate = generalLedger.FromDate,
                        ToDate = generalLedger.ToDate,
                        Balance = (decimal?) Math.Round((double) (balance/currencyValue), 2),
                        Particulars = generalLedger.Particulars
                    };

                    glModel.Add(GeneralLedgerDetailModel);
                }

                var ledger = new GeneralLedgerDetailModel();
                ledger.GLName = "Opening Balance";
                ledger.Balance = openingBalance ?? (decimal?) 0.00;

                var spGeneralLedgerDetailReportResult = glModel.FirstOrDefault();

                if (spGeneralLedgerDetailReportResult != null)
                {
                    ledger.SectorName = spGeneralLedgerDetailReportResult.SectorName;
                    ledger.GLNameSearch = spGeneralLedgerDetailReportResult.GLNameSearch;
                    ledger.FromDate = spGeneralLedgerDetailReportResult.FromDate;
                    ledger.ToDate = spGeneralLedgerDetailReportResult.ToDate;
                }

                ledger.FromDate = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                ledger.ToDate = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                decimal? accCode = Convert.ToDecimal(accountCode);
                ledger.GLNameSearch = _context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode).AccountName;

                glModel.Insert(0, ledger);
            }

            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return glModel;
        }

        public List<ChartOfAccountsReportModel> GetChartOfAccountsReportData()
        {
            List<ChartOfAccountsReportModel> chartofaccounts;

            chartofaccounts = _context.Database.SqlQuery<ChartOfAccountsReportModel>("ACCChartOfAccountReport").ToList();

            return chartofaccounts;
        }

        public long GetVoucherIdbyVoucherNo(int? voucherNo)
        {
            var accVoucherMaster = _context.Acc_VoucherMaster.FirstOrDefault(p => p.VoucherNo == voucherNo);
            if (accVoucherMaster != null) return accVoucherMaster.Id;
            return 1;
        }

        public DataTable GetReport()
        {
            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;


            const string cmdText = @"select * from Acc_CompanySector";
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }

            return table;
        }

        public DataTable GetGeneralLedger(Acc_ReportViewModel iObj)
        {
            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;
            string cmdText = @"select * from VW_ACC_Transactions where GLID is not null ";
            if (iObj.SectorCode != "" && iObj.SectorCode != "")
                cmdText = cmdText + " and SectorId=" + iObj.SectorCode + "";
            if (iObj.GLId != "" && iObj.GLId != "" && iObj.GLId != null)
                cmdText = cmdText + " and AccountCode='" + iObj.GLId + "'";
            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.StartDate + "', 103) and CONVERT(datetime,'" + iObj.EndDate + "', 103) ";

            cmdText = cmdText + "ORDER BY CONVERT(Date,VoucherDate,103), VoucherNo ";

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public DataTable GetTrialbalanceCostCentre(Acc_ReportViewModel iObj)
        {
            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;

            int costcentreId = 0;
            if (iObj.CostCentreID != null)
                costcentreId = Convert.ToInt32(iObj.CostCentreID);

            string costCentre = _context.Acc_CostCentreMultiLayer.FirstOrDefault(p => p.Id == costcentreId).ItemName;
            costCentre = costcentreId.ToString();

            string cmdText = @"select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ," +
                             " isnull(( Select sum(A.Debit) - sum(A.Credit) From VW_ACC_Transactions A where A.SubGrpControlCode= H.SubGrpControlCode and A.SectorId =" + iObj.SectorCode + " and A.CostCentreId ='" + costCentre + "' and CONVERT(datetime,A.VoucherDate, 103) <= CONVERT(datetime,'" + iObj.StartDate + "', 103)),0 ) as OpeningBalance," +
                             " isnull(( Select sum(A.Debit) - sum(A.Credit) From VW_ACC_Transactions A where A.SubGrpControlCode= H.SubGrpControlCode and A.SectorId =" + iObj.SectorCode + " and A.CostCentreId ='" + costCentre + "' and CONVERT(datetime,A.VoucherDate, 103) <= CONVERT(datetime,'" + iObj.EndDate + "', 103) ),0 ) as ClosingBalance" +
                             " from VW_ACC_ChartofAccount as H where ClsControlCode in(1,2)" +
                             " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName";
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }

            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public DataTable GetTrialbalanceNew(Acc_ReportViewModel iObj)
        {
            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;
            string cmdText = @"";

            cmdText = cmdText + @"select A.*,isnull(b.OpeningBalance,0) OpeningBalance,isnull(B.Debit,0) Debit,isnull(B.Credit,0) Credit,isnull(B.ClosingBalance,0)  ClosingBalance from (
                        select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName 
                        from VW_ACC_ChartofAccount where ClsControlCode in(1,2) 
                        group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName 
                        ) A left outer join 
                        (";



            if (iObj.TrialBalanceLebel == 1)
                cmdText = cmdText + "select ClsControlCode,ClsControlName, ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = cmdText + "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText = cmdText +
                          "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6) //Balance Sheet or Income Statement
                cmdText = cmdText +
                          "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText = cmdText +
                          "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText = cmdText +
                          "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            cmdText = cmdText +
                      @",sum(cast(OpeningBalance as numeric(12,2))) OpeningBalance,sum(Debit) Debit,sum(Credit) Credit,
            case 
            when SUBSTRING(CAST(ClsControlCode as NCHAR) ,1,1) in('1','4') then 
            sum(cast(OpeningBalance as numeric(12,2)))+sum(Debit)-sum(Credit)
            else sum(cast(OpeningBalance as numeric(12,2)))+sum(Credit)-sum(Debit)
            end as ClosingBalance 
            from (
            select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,
            ControlCodeName,AccountCode,AccountName, 
            isnull(OP.OpeningBalance,0) OpeningBalance 
            ,sum(Debit) Debit,sum(Credit) Credit
            from VW_ACC_Transactions A left outer join(
            select A.SectorId OpSectorID,A.GLCode OpGLID, 
            case 
            when SUBSTRING(CAST(AccountCode as NCHAR) ,1,1) in('1','4') 
            then (isnull(B.Debit,0)-isnull(B.Credit,0))+(A.Debit-A.Credit)
            else (isnull(B.Credit,0)-isnull(B.Debit,0))+(A.Credit-A.Debit)
            end as OpeningBalance 
            from (
            select A.SectorId,A.GLCode,A.AccountCode, sum(A.Debit) Debit,sum(A.Credit) Credit from VW_ACC_Transactions A where GLID is not null  ";
            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,A.VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.OpStartDate + "', 103) and CONVERT(datetime,'" + iObj.OpEndDate + "', 103)-1 ";
            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and A.SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and A.FinancialPeriodId=" + iObj.FpId + "";

            cmdText = cmdText + @"group by AccountCode,A.GLCode,A.SectorId ) A left outer join Acc_OpeningClosing B on A.GLCode=B.GlId and A.SectorId=B.SectorId 
            ) Op on Op.OpGLID=A.GLCode and Op.OpSectorID=A.SectorId
            where GLID is not null ";

            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,'" + iObj.StartDate + "', 103) and CONVERT(datetime,'" + iObj.EndDate + "', 103) ";

            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and FinancialPeriodId=" + iObj.FpId + "";

            if (iObj.GLId != "" && iObj.GLId != null)
                cmdText = cmdText + " and ClsControlCode in(" + iObj.GLId + ")";
            if (iObj.cotrolcode != "" && iObj.cotrolcode != null)
                cmdText = cmdText + " and ControlContolCode in(" + iObj.cotrolcode + ")";
            if (iObj.AccountCode != "" && iObj.AccountCode != null)
                cmdText = cmdText + " and AccountCode in(" + iObj.AccountCode + ")";

            cmdText = cmdText + @"group by OP.OpeningBalance,ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,
            SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ) A";

            if (iObj.TrialBalanceLebel == 1)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";


            cmdText = cmdText + ") B on A.SubGrpControlCode=B.SubGrpControlCode";



            cmdText = "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ," +
                      " isnull(( Select sum(A.Debit) - sum(A.Credit) From VW_ACC_Transactions A where A.SubGrpControlCode= H.SubGrpControlCode and A.SectorId =" + iObj.SectorCode + " and CONVERT(datetime,A.VoucherDate, 103) <= CONVERT(datetime,'" + iObj.StartDate + "', 103)),0 ) as OpeningBalance," +
                      " isnull(( Select sum(A.Debit) - sum(A.Credit) From VW_ACC_Transactions A where A.SubGrpControlCode= H.SubGrpControlCode and A.SectorId =" + iObj.SectorCode + " and CONVERT(datetime,A.VoucherDate, 103) <= CONVERT(datetime,'" + iObj.EndDate + "', 103) ),0 ) as ClosingBalance" +
                      " from VW_ACC_ChartofAccount as H where ClsControlCode in(1,2) " +
                      " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName";
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }

            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public DataTable GetTrialBalance(Acc_ReportViewModel iObj)
        {
            var table = new DataTable();
            var connection = (SqlConnection)_context.Database.Connection;
            string cmdText = @"";

            if (iObj.TrialBalanceLebel == 1)
                cmdText = "select ClsControlCode,ClsControlName, ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6) //Balance Sheet or Income Statement
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            cmdText = cmdText +
                      @",sum(cast(OpeningBalance as numeric(12,2))) OpeningBalance,sum(Debit) Debit,sum(Credit) Credit,
        case 
        when SUBSTRING(CAST(ClsControlCode as NCHAR) ,1,1) in('1','4','2','3') then 
        sum(cast(OpeningBalance as numeric(12,2)))+sum(Debit)-sum(Credit)
        else sum(cast(OpeningBalance as numeric(12,2)))+sum(Credit)-sum(Debit)
        end as ClosingBalance 
from (
select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,
ControlCodeName,AccountCode,AccountName, 
isnull(OP.OpeningBalance,0) OpeningBalance 
,sum(Debit) Debit,sum(Credit) Credit
from VW_ACC_Transactions A left outer join(
select A.SectorId OpSectorID,A.GLCode OpGLID, 
case 
when SUBSTRING(CAST(AccountCode as NCHAR) ,1,1) in('1','4','2','3') 
then (isnull(B.Debit,0)-isnull(B.Credit,0))+(A.Debit-A.Credit)
else (isnull(B.Credit,0)-isnull(B.Debit,0))+(A.Credit-A.Debit)
end as OpeningBalance 
from (
select A.SectorId,A.GLCode,A.AccountCode, sum(A.Debit) Debit,sum(A.Credit) Credit from VW_ACC_Transactions A where GLID is not null  ";
            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,A.VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.OpStartDate + "', 103) and CONVERT(datetime,'" + iObj.OpEndDate + "', 103)-1 ";
            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and A.SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and A.FinancialPeriodId=" + iObj.FpId + "";

            cmdText = cmdText + @"group by AccountCode,A.GLCode,A.SectorId ) A left outer join Acc_OpeningClosing B on A.GLCode=B.GlId and A.SectorId=B.SectorId 
) Op on Op.OpGLID=A.GLCode and Op.OpSectorID=A.SectorId
where GLID is not null ";

            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.StartDate + "', 103) and CONVERT(datetime,'" + iObj.EndDate + "', 103) ";
            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and FinancialPeriodId=" + iObj.FpId + "";

            if (iObj.GLId != "" && iObj.GLId != null)
                cmdText = cmdText + " and ClsControlCode in(" + iObj.GLId + ")";
            if (iObj.cotrolcode != "" && iObj.cotrolcode != null)
                cmdText = cmdText + " and ControlContolCode in(" + iObj.cotrolcode + ")";
            if (iObj.AccountCode != "" && iObj.AccountCode != null)
                cmdText = cmdText + " and AccountCode in(" + iObj.AccountCode + ")";

            cmdText = cmdText + @"group by OP.OpeningBalance,ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,
SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ) A";

            if (iObj.TrialBalanceLebel == 1)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }

            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public DataTable GetTrialBalanceCostCentre(Acc_ReportViewModel iObj)
        {
            int costcentreId = 0;
            if (iObj.CostCentreID != null)
                costcentreId = Convert.ToInt32(iObj.CostCentreID);

            string costCentre = _context.Acc_CostCentreMultiLayer.FirstOrDefault(p => p.Id == costcentreId).ItemName;
            costCentre = costcentreId.ToString();

            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;
            string cmdText = @"";

            if (iObj.TrialBalanceLebel == 1)
                cmdText = "select ClsControlCode,ClsControlName, ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6) //Balance Sheet or Income Statement
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText =
                    "select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            cmdText = cmdText +
                      @",sum(cast(OpeningBalance as numeric(12,2))) OpeningBalance,sum(Debit) Debit,sum(Credit) Credit,
        case 
        when SUBSTRING(CAST(ClsControlCode as NCHAR) ,1,1) in('1','4') then 
        sum(cast(OpeningBalance as numeric(12,2)))+sum(Debit)-sum(Credit)
        else sum(cast(OpeningBalance as numeric(12,2)))+sum(Credit)-sum(Debit)
        end as ClosingBalance 
from (
select ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,
ControlCodeName,AccountCode,AccountName, 
isnull(OP.OpeningBalance,0) OpeningBalance 
,sum(Debit) Debit,sum(Credit) Credit
from VW_ACC_Transactions A left outer join(
select A.SectorId OpSectorID,A.GLCode OpGLID, 
case 
when SUBSTRING(CAST(AccountCode as NCHAR) ,1,1) in('1','4') 
then (isnull(B.Debit,0)-isnull(B.Credit,0))+(A.Debit-A.Credit)
else (isnull(B.Credit,0)-isnull(B.Debit,0))+(A.Credit-A.Debit)
end as OpeningBalance 
from (
select A.SectorId,A.GLCode,A.AccountCode, sum(A.Debit) Debit,sum(A.Credit) Credit from VW_ACC_Transactions A where GLID is not null  ";
            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,A.VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.OpStartDate + "', 103) and CONVERT(datetime,'" + iObj.OpEndDate + "', 103)-1 ";
            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and A.SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and A.FinancialPeriodId=" + iObj.FpId + "";

            cmdText = cmdText + @"group by AccountCode,A.GLCode,A.SectorId ) A left outer join Acc_OpeningClosing B on A.GLCode=B.GlId and A.SectorId=B.SectorId 
) Op on Op.OpGLID=A.GLCode and Op.OpSectorID=A.SectorId
where GLID is not null ";

            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.StartDate + "', 103) and CONVERT(datetime,'" + iObj.EndDate + "', 103) ";
            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            //cmdText = cmdText + " AND CostCentreId = '8-Knit-CostCentre3' ";  // Newly added for Cost Centre

            cmdText = cmdText + " AND CostCentreId ='" + costCentre + "' ";

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and FinancialPeriodId=" + iObj.FpId + "";

            if (iObj.GLId != "" && iObj.GLId != null)
                cmdText = cmdText + " and ClsControlCode in(" + iObj.GLId + ")";
            if (iObj.cotrolcode != "" && iObj.cotrolcode != null)
                cmdText = cmdText + " and ControlContolCode in(" + iObj.cotrolcode + ")";
            if (iObj.AccountCode != "" && iObj.AccountCode != null)
                cmdText = cmdText + " and AccountCode in(" + iObj.AccountCode + ")";

            cmdText = cmdText + @"group by OP.OpeningBalance,ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,
SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ) A";

            if (iObj.TrialBalanceLebel == 1)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName ";
            if (iObj.TrialBalanceLebel == 2)
                cmdText = cmdText + " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName ";
            if (iObj.TrialBalanceLebel == 3)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName ";
            if (iObj.TrialBalanceLebel == 6)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 4)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName ";
            if (iObj.TrialBalanceLebel == 5)
                cmdText = cmdText +
                          " group by ClsControlCode,ClsControlName,GrpControlCode,GrpControlName,SubGrpControlCode,SubGrpControlName,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }

            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public DataTable GetCashFlowData(Acc_ReportViewModel iObj)
        {
            var table = new DataTable();
            var connection = (SqlConnection) _context.Database.Connection;
            string cmdText = @"";


            cmdText =
                "select CashFlow,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            cmdText = cmdText +
                      @",sum(cast(OpeningBalance as numeric(12,2))) OpeningBalance,sum(Debit) Debit,sum(Credit) Credit,
case 
when SUBSTRING(CAST(ControlContolCode as NCHAR) ,1,1) in('1','4') then 
sum(cast(OpeningBalance as numeric(12,2)))+sum(Debit)-sum(Credit)
else sum(cast(OpeningBalance as numeric(12,2)))+sum(Credit)-sum(Debit)
end as ClosingBalance 
from 
(
select 
case when  VoucherType in('BP','CP') then 'Outflow'
when  VoucherType in('BR','CR') then 'Inflow' 
end as CashFlow,A.*
from VW_ACC_Transactions A  where VoucherType not in('JV','BC','CB','BB') and isnull(AccountType,'') not in('Bank','Cash')
) A where GLID is not null  ";

            if (iObj.StartDate != "" && iObj.StartDate != null)
                cmdText = cmdText + " and CONVERT(datetime,VoucherDate, 103) between CONVERT(datetime,'" +
                          iObj.StartDate + "', 103) and CONVERT(datetime,'" + iObj.EndDate + "', 103) ";

            if (iObj.SectorCode != "" && iObj.SectorCode != null)
                cmdText = cmdText + " and SectorId=" + iObj.SectorCode + ""; // FinancialPeriodId

            if (iObj.FpId != "" && iObj.FpId != null)
                cmdText = cmdText + " and FinancialPeriodId=" + iObj.FpId + "";

            if (iObj.GLId != "" && iObj.GLId != null)
                cmdText = cmdText + " and ClsControlCode in(" + iObj.GLId + ")";

            cmdText = cmdText +
                      " group by CashFlow,ControlContolCode,ControlCodeName,AccountCode,AccountName ";

            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                table = null;
            }

            finally
            {
                if (connection != null) connection.Close();
            }

            return table;
        }

        public string GetVoucherNoByRefNo(long refNo)
        {
            var voucherNo = "";
            try
            {
                var accVoucherMaster = _context.Acc_VoucherMaster.FirstOrDefault(p => p.VoucherNo == refNo);
                if (accVoucherMaster != null)
                {
                    voucherNo = accVoucherMaster.VoucherRefNo;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return voucherNo;
        }

        public int? GetActiveCurrencyByVoucherMasterId(long id)
        {
            return _context.Acc_VoucherMaster.Find(id).ActiveCurrencyId;
        }

        public GeneralLedgerDetailModel GetOpeningBalanceByGlId(string sectorId, DateTime fromDate, DateTime toDate, string accountCode)
        {
            decimal? OpeningBalance = 0;
            int? sector = Convert.ToInt32(sectorId);
            decimal accCode = Convert.ToDecimal(accountCode);

            string accountName = _context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode).AccountName;
            int? glId = _context.Acc_GLAccounts.FirstOrDefault(p => p.AccountCode == accCode).Id;

            var temp = from p in _context.Acc_VoucherMaster
                join q in _context.Acc_VoucherDetail on p.Id equals q.RefId into pq
                from g in pq.DefaultIfEmpty()
                where p.VoucherDate >= fromDate && p.VoucherDate < toDate && p.SectorId == sector && g.GLID == glId
                select new
                {
                    Debit = g.Debit,
                    Credit = g.Credit
                };

            foreach (var t in temp)
            {
                OpeningBalance = OpeningBalance + t.Debit - t.Credit;
            }

            GeneralLedgerDetailModel ledger = new GeneralLedgerDetailModel();
            ledger.Balance = OpeningBalance;
            ledger.GLName = accountName;

            return ledger;
        }

        public Acc_FinancialPeriod GetFinancialPeriod(string financialPeriodId)
        {
            int financialId = Convert.ToInt32(financialPeriodId);
            var financialPeriod = _context.Acc_FinancialPeriod.FirstOrDefault(p => p.Id == financialId);
            return financialPeriod;
        }

        public Acc_FinancialPeriod GetActiveFinancialPeriod()
        {
            var financialPeriod = _context.Acc_FinancialPeriod.FirstOrDefault(p => p.ActiveStatus.Value);
            return financialPeriod;
        }

        public int GetFinancialPeriodId(DateTime fromDate, DateTime toDate)
        {
            
            var financialPeriod = _context.Acc_FinancialPeriod.FirstOrDefault(p => p.PeriodStartDate <= fromDate && p.PeriodEndDate >= toDate);
            return financialPeriod.Id;
        }

        public DataTable GetGetCashBook(DateTime fromDate, DateTime toDate, int glId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spAccCashBook"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GLID", SqlDbType.Int).Value = glId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetControlTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetControlTrialBalance"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetGLTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetGLTrialBalance"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetSubGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetSubGroupTrialBalance"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetGroupTrialBalance(int SectorId, DateTime fromDate, DateTime toDate, int FpId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetGroupTrialBalance"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetControlLedger(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetControlLedger"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                cmd.Parameters.Add("@ControlCode", SqlDbType.Int).Value = ControlCode;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetBalanceSheetNote(int SectorId, DateTime fromDate, DateTime toDate, int FpId, int ControlCode)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetBalanceSheetNote"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;
                cmd.Parameters.Add("@FiscalPeriodId", SqlDbType.Int).Value = FpId;
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                cmd.Parameters.Add("@ControlCode", SqlDbType.Int).Value = ControlCode;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetAgingData(int SectorId, DateTime fromDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("GetAgingReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmd.Parameters.Add("@CompanySectorId", SqlDbType.Int).Value = SectorId;            
                           
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}