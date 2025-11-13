using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class KnittingInvoiceViewModel
    {
        public DateTime? InvoiceDate { get; set; }
        public string SerarchString { get; set; }
        public List<KnittingAccountInvoice> knittingBills { get; set; }
        public KnittingInvoiceViewModel()
        {
            knittingBills = new List<KnittingAccountInvoice>();
        }
    }
}