using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
   public class DyeingAccountInvoice
    {
        public string ProgramRefId { get; set; }
        public string RefId { get; set; }
        public string Party { get; set; }
        public int? DglId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string AccountName { get; set; }
        public string InvoiceNo { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double BillAmount { get; set; }
    }
}
