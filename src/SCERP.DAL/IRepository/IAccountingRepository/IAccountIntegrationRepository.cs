using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.IRepository.IAccountingRepository
{
    public interface IAccountIntegrationRepository
    {
        List<KnittingAccountInvoice> GetKnittingPayableBillInvoice(DateTime? invoiceDate,string searchString);
        List<DyeingAccountInvoice> GetDyingBillInvoices(DateTime? invoiceDate);
        KnittingAccountInvoice GetKnittingVoucher(string refId);
        List<PrintEmbAccountInvoice> GetPrintEmbBillInvoice(string compId,string processRefId);
        List<KnittingAccountInvoice> GetKnittingReceivableBillInvoice(DateTime? invoiceDate, string searchString);
        KnittingAccountInvoice GetKnittingReceivableInvoice(string refId);
        DyeingAccountInvoice GetVwDyingBillInvoices(string key);
        PrintEmbAccountInvoice GetPrintEmbBillVoucher(string key);
    }
}
