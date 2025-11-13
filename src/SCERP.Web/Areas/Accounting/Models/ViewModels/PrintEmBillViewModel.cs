using SCERP.Common;
using SCERP.Model.AccountingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class PrintEmBillViewModel
    {
        public DateTime? InvoiceDate { get; set; }
        public string ProcessRefId { get; set; }
        public List<PrintEmbAccountInvoice> PrintEmbBills { get; set; }
        public PrintEmBillViewModel()
        {
            PrintEmbBills = new List<PrintEmbAccountInvoice>();
        }

        public IEnumerable<SelectListItem> ProcessTypeSelectListItem
        {
            get
            {
                return new SelectList(new[] { new { ProcessRefId = ProcessCode.PRINTING, Name = "Printing Payable" }, new { ProcessRefId = ProcessCode.EMBROIDARY, Name = "Embroider Payable" } }, "ProcessRefId", "Name");
            }
        }
    }
}