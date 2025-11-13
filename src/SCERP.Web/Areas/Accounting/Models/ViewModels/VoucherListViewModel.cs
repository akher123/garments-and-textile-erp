using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;
using WebGrease.Activities;

namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class VoucherListViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string VoucherType { get; set; }
        public long VoucherNo { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
    }
}