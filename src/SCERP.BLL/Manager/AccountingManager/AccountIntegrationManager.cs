using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model;
using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class AccountIntegrationManager: IAccountIntegrationManager
    {
        private readonly IAccountIntegrationRepository _accountIntegrationRepository;
        public AccountIntegrationManager(IAccountIntegrationRepository accountIntegrationRepository)
        {
            _accountIntegrationRepository = accountIntegrationRepository;
        }

        public List<DyeingAccountInvoice> GetDyingBillInvoices(DateTime? invoiceDate)
        {
            return _accountIntegrationRepository.GetDyingBillInvoices(invoiceDate);
        }

        public Acc_VoucherMaster GetDyingBillVoucher(string key, int tableId)
        {
            Acc_VoucherMaster vm = new Acc_VoucherMaster();
            DyeingAccountInvoice kac = _accountIntegrationRepository.GetVwDyingBillInvoices(key);
            vm.Particulars = kac.Party;
            vm.CheckNo = kac.InvoiceNo;
            vm.VoucherRefNo = kac.InvoiceNo;
            vm.VoucherDate = kac.InvoiceDate.GetValueOrDefault();
            vm.IntRefId = kac.RefId;
            vm.CheckDate = kac.InvoiceDate.ToString();
            vm.IntType = (int)BillTable.Inventory_FinishFabricIssue;
            vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = kac.DglId, IntRefId = kac.RefId, IntType = tableId, Debit = Convert.ToDecimal(kac.BillAmount), Credit = 0, Particulars = kac.AccountName, FirstCurValue = 1, CostCentreId = "6", Acc_GLAccounts = new Acc_GLAccounts() { Id = kac.DglId ?? 0, AccountName = kac.AccountName } });
            vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = (int)AccountRevenue.DyeingChargeIncome, IntRefId = kac.RefId, IntType = tableId, Debit = 0, Credit = Convert.ToDecimal(kac.BillAmount), Particulars = AccountRevenue.DyeingChargeIncome.ToString(), CostCentreId = "6", FirstCurValue = 1, Acc_GLAccounts = new Acc_GLAccounts() { Id = (int)AccountRevenue.DyeingChargeIncome, AccountName = AccountRevenue.DyeingChargeIncome.ToString() } });
            return vm;
        }

        public List<KnittingAccountInvoice> GetKnittingBillInvoice(DateTime? invoiceDate,string searchString, int tableId)
        {
            List<KnittingAccountInvoice> knittingAccountInvoices;
            switch (tableId)
            {
                case (int)BillTable.PROD_KnittingRollIssue:
                    knittingAccountInvoices= _accountIntegrationRepository.GetKnittingPayableBillInvoice(invoiceDate, searchString);
                    break;
                case (int)BillTable.Inventory_GreyIssue:
                    knittingAccountInvoices= _accountIntegrationRepository.GetKnittingReceivableBillInvoice(invoiceDate, searchString);
                    break;
                default:
                    knittingAccountInvoices = new List<KnittingAccountInvoice>();
                    break;
            }
            return knittingAccountInvoices;
        }

      
        public Acc_VoucherMaster GetKnittingVoucher(string refId,int tableId)
        {
            Acc_VoucherMaster vm = new Acc_VoucherMaster();
            KnittingAccountInvoice kac;
            switch (tableId)
            {
                case (int)BillTable.PROD_KnittingRollIssue:
                  kac = _accountIntegrationRepository.GetKnittingVoucher(refId);
                    break;
                case (int)BillTable.Inventory_GreyIssue:
                     kac = _accountIntegrationRepository.GetKnittingReceivableInvoice(refId);
                    break;
                default:
                    kac = new KnittingAccountInvoice();
                    break;
            }
            vm.Particulars = kac.Party;
            vm.CheckNo = kac.InvoiceNo;
            vm.VoucherRefNo = kac.InvoiceNo;
            vm.VoucherDate = kac.InvoiceDate.GetValueOrDefault();
            vm.IntRefId = kac.RefId;
            vm.CheckDate = kac.InvoiceDate.ToString();
            if (kac.BillType ==1) // Payable Bill
            {
                vm.IntType = (int)BillTable.PROD_KnittingRollIssue;
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = (int)AccountExpenses.KnittingChargeExpenses, IntRefId = kac.RefId, IntType = tableId, Debit = Convert.ToDecimal(kac.BillAmount), Credit = 0, Particulars = AccountExpenses.KnittingChargeExpenses.ToString(), CostCentreId = "6", FirstCurValue = 1, Acc_GLAccounts = new Acc_GLAccounts() { Id = (int)AccountExpenses.KnittingChargeExpenses, AccountName = AccountExpenses.KnittingChargeExpenses.ToString() } });
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = kac.KglId, IntRefId= kac.RefId, IntType= tableId, Debit = 0, Credit = Convert.ToDecimal(kac.BillAmount), Particulars = kac.AccountName, FirstCurValue = 1, CostCentreId = "6", Acc_GLAccounts = new Acc_GLAccounts() { Id = kac.KglId ?? 0, AccountName = kac.AccountName } });
                
            }
            if(kac.BillType == 2) // Receivable Bill
            {
                vm.IntType = (int)BillTable.Inventory_GreyIssue;
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = kac.KglId, IntRefId = kac.RefId, IntType = tableId, Debit = Convert.ToDecimal(kac.BillAmount), Credit = 0, Particulars = kac.AccountName, FirstCurValue = 1, CostCentreId = "6", Acc_GLAccounts = new Acc_GLAccounts() { Id = kac.KglId ?? 0, AccountName = kac.AccountName } });
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = (int)AccountRevenue.KnittingChargeIncome, IntRefId = kac.RefId, IntType = tableId, Debit = 0, Credit = Convert.ToDecimal(kac.BillAmount), Particulars = AccountRevenue.KnittingChargeIncome.ToString(), CostCentreId = "6", FirstCurValue = 1, Acc_GLAccounts = new Acc_GLAccounts() { Id = (int)AccountRevenue.KnittingChargeIncome, AccountName = AccountRevenue.KnittingChargeIncome.ToString() } });

            }

            return vm;
        }

        public List<PrintEmbAccountInvoice> GetPrintEmbBillInvoice(string compId, string processRefId)
        {
            return _accountIntegrationRepository.GetPrintEmbBillInvoice(compId, processRefId);
        }

        public Acc_VoucherMaster GetPrintEmbBillVoucher(string key, int tableId)
        {
            Acc_VoucherMaster vm = new Acc_VoucherMaster();
            PrintEmbAccountInvoice kac = _accountIntegrationRepository.GetPrintEmbBillVoucher(key);
            vm.Particulars = kac.Party;
            vm.CheckNo = kac.InvoiceNo;
            vm.VoucherRefNo = kac.InvoiceNo;
            vm.VoucherDate = kac.InvoiceDate.GetValueOrDefault();
            vm.IntRefId = kac.RefId;
            vm.CheckDate = kac.InvoiceDate.ToString();
            vm.IntType = (int)BillTable.PROD_ProcessReceive;
            if (kac.ProcessRefId == ProcessCode.PRINTING)
            {
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = kac.GlId, IntRefId = kac.RefId, IntType = tableId, Debit = Convert.ToDecimal(kac.BillAmount), Credit = 0, Particulars = kac.AccountName, FirstCurValue = 1, CostCentreId = "6", Acc_GLAccounts = new Acc_GLAccounts() { Id = kac.GlId ?? 0, AccountName = kac.AccountName } });
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = (int)AccountExpenses.PrintingChargeExpenses, IntRefId = kac.RefId, IntType = tableId, Debit = 0, Credit = Convert.ToDecimal(kac.BillAmount), Particulars = AccountExpenses.PrintingChargeExpenses.ToString(), CostCentreId = "6", FirstCurValue = 1, Acc_GLAccounts = new Acc_GLAccounts() { Id = (int)AccountExpenses.PrintingChargeExpenses, AccountName = AccountExpenses.PrintingChargeExpenses.ToString() } });
            }

            if (kac.ProcessRefId==ProcessCode.EMBROIDARY)
            {
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = kac.GlId, IntRefId = kac.RefId, IntType = tableId, Debit = Convert.ToDecimal(kac.BillAmount), Credit = 0, Particulars = kac.AccountName, FirstCurValue = 1, CostCentreId = "6", Acc_GLAccounts = new Acc_GLAccounts() { Id = kac.GlId ?? 0, AccountName = kac.AccountName } });
                vm.Acc_VoucherDetail.Add(new Acc_VoucherDetail() { GLID = (int)AccountExpenses.EmbroiderChargeExpenses, IntRefId = kac.RefId, IntType = tableId, Debit = 0, Credit = Convert.ToDecimal(kac.BillAmount), Particulars = AccountExpenses.EmbroiderChargeExpenses.ToString(), CostCentreId = "6", FirstCurValue = 1, Acc_GLAccounts = new Acc_GLAccounts() { Id = (int)AccountExpenses.EmbroiderChargeExpenses, AccountName = AccountExpenses.EmbroiderChargeExpenses.ToString() } });
            }
         
            return vm;
        }
    }
}
