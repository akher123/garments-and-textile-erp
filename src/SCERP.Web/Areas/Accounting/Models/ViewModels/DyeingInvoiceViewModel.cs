using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class DyeingInvoiceViewModel
    {
        public DateTime? InvoiceDate { get; set; }
        public List<DyeingAccountInvoice> DyeingAccountInvoices { get; set; }
        public DyeingInvoiceViewModel()
        {
            DyeingAccountInvoices = new List<DyeingAccountInvoice>();
        }
    }
}