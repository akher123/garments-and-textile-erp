using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SCERP.DAL.Repository.AccountingRepository
{
    public class AccountIntegrationRepository: IAccountIntegrationRepository
    {
        private readonly SCERPDBContext _context;
        public AccountIntegrationRepository(SCERPDBContext context)
        {
            _context = context;
        }

        public List<DyeingAccountInvoice> GetDyingBillInvoices(DateTime? invoiceDate)
        {
            string spName = "exec spDyeingFinishFabricReceivableBills @InvoiceDate='{0}'";
            return _context.Database.SqlQuery<DyeingAccountInvoice>(String.Format(spName, invoiceDate)).ToList();
        }

      
        public List<KnittingAccountInvoice> GetKnittingPayableBillInvoice(DateTime? invoiceDate,string searchString)
        {
            string spName = "exec spKnittingPayableBillsInvoice @InvoiceDate='{0}',@SearchString='{1}'";
            return _context.Database.SqlQuery<KnittingAccountInvoice>(String.Format(spName, invoiceDate, searchString)).ToList();
        }

        public List<KnittingAccountInvoice> GetKnittingReceivableBillInvoice(DateTime? invoiceDate,string searchString)
        {
            string spName = "exec spKnittingReceivableBills @InvoiceDate='{0}',@SearchString='{1}'";
            return _context.Database.SqlQuery<KnittingAccountInvoice>(String.Format(spName, invoiceDate, searchString)).ToList();
        }

        public KnittingAccountInvoice GetKnittingReceivableInvoice(string refId)
        {

            string sql = "select * from VwKnittingReceivable  where RefId ='{0}'";
            return _context.Database.SqlQuery<KnittingAccountInvoice>(String.Format(sql, refId)).FirstOrDefault();
        }

        public KnittingAccountInvoice GetKnittingVoucher(string refId)
        {
            string sql = "select * from VwKnittingPayableBill  where RefId ='{0}'";
            return _context.Database.SqlQuery<KnittingAccountInvoice>(String.Format(sql, refId)).FirstOrDefault();
        }

        public List<PrintEmbAccountInvoice> GetPrintEmbBillInvoice(string compId,string processRefId)
        {
            string spName = "exec PrintEmbBills @CompId='{0}',@ProcessRefId='{1}'";
            return _context.Database.SqlQuery<PrintEmbAccountInvoice>(String.Format(spName, compId, processRefId)).ToList();
        }

        public DyeingAccountInvoice GetVwDyingBillInvoices(string refId)
        {
            string sql = "select * from DyeingFinishFabricReceivableBills  where RefId ='{0}'";
            return _context.Database.SqlQuery<DyeingAccountInvoice>(String.Format(sql, refId)).FirstOrDefault();
        }

        public PrintEmbAccountInvoice GetPrintEmbBillVoucher(string refId)
        {
            string sql = "select * from VwPrintEmbPayableBill  where RefId ='{0}'";
            return _context.Database.SqlQuery<PrintEmbAccountInvoice>(String.Format(sql, refId)).FirstOrDefault();
        }


    }
}
