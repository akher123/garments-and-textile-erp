using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class PrintEmbAccountInvoice
    {

        public string RefId { get; set; }
        public string ProcessRefId { get; set; }
        public string ProcessTitle { get; set; }
        public string Party { get; set; }
        public int? GlId { get; set; }
        public string AccountName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int Qty { get; set; }
        public double Rate { get; set; }
        public double BillAmount { get; set; }
    }
}
