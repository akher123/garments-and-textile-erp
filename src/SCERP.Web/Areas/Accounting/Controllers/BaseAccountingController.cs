using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class BaseAccountingController : BaseController
    {
        #region Accounting

        public ICompanySectorManager CompanySectorManager
        {
            get { return Manager.CompanySectorManager; }
        }

        public ICostCentreManager CostCentreManager
        {
            get { return Manager.CostCentreManager; }
        }

        public IFinancialPeriodManager FinancialPeriodManager
        {
            get { return Manager.FinancialPeriodManager; }
        }

        public IJournalVoucherEntryManager JournalVoucherEntryManager
        {
            get { return Manager.JournalVoucherEntryManager; }
        }

        public ICashVoucherEntryManager CashVoucherEntryManager
        {
            get { return Manager.CashVoucherEntryManager; }
        }

        //public IControlAccountManager ControlAccountManager
        //{
        //    get { return Manager.ControlAccountManager; }
        //}

        public IBankVoucherEntryManager BankVoucherEntryManager
        {
            get { return Manager.BankVoucherEntryManager; }
        }

        public IContraVoucherEntryManager ContraVoucherEntryManager
        {
            get { return Manager.ContraVoucherEntryManager; }
        }

        public IReportAccountManger ReportAccountManger
        {
            get { return Manager.ReportAccountManger; }
        }

        public IOpeningBalaceManager OpeningBalaceManager
        {
            get { return Manager.OpeningBalaceManager; }
        }

        public IVoucherListManager VoucherListManager
        {
            get { return Manager.VoucherListManager; }
        }

        public IBankReconcilationManager BankReconcilationManager
        {
            get { return Manager.BankReconcilationManager; }
        }

        public IBankReconcilationListManager BankReconcilationListManager
        {
            get { return Manager.BankReconcilationListManager; }
        }

        public IDepreciationChartManager DepreciationChartManager
        {
            get { return Manager.DepreciationChartManager; }
        }
        //public IGLAccountManager GlAccountManager
        //{
        //    get { return Manager.GlAccountManager; }
        //}
        //public IVoucherMasterManager VoucherMasterManager
        //{
        //    get { return Manager.VoucherMasterManager; }
        //}

        public IAccCurrencManager AccCurrencManager
        {
            get { return Manager.AccCurrencManager; }
        }

        public IGLAccountHiddenManager GLAccountHiddenManager
        {
            get { return Manager.GLAccountHiddenManager; }
        }

        #endregion
    }
}