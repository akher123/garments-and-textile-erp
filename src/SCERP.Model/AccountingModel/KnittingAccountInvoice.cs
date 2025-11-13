using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class KnittingAccountInvoice
    {
        public long Id { get; set; }
        public string ProgramRefId { get; set; }
        public string RefId { get; set; }
        public int BillType { get; set; }
        public string Party { get; set; }
        public int? KglId { get; set; }
        public string AccountName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double Qty { get; set; }
        public decimal BillAmount { get; set; }


    }
}
