using SCERP.Model;
using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IAccountIntegrationManager
    {
        List<KnittingAccountInvoice> GetKnittingBillInvoice(DateTime? invoiceDate, string searchString,int tableId);
        List<DyeingAccountInvoice> GetDyingBillInvoices(DateTime? invoiceDate);
        Acc_VoucherMaster GetKnittingVoucher(string key,int tableId);
        List<PrintEmbAccountInvoice> GetPrintEmbBillInvoice(string compId,string processRefId);
        Acc_VoucherMaster GetDyingBillVoucher(string key, int tableId);
        Acc_VoucherMaster GetPrintEmbBillVoucher(string key, int tableId);
    }
}
