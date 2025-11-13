using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;


namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class CashVoucherEntryViewModel : Acc_VoucherMaster
    {
        public Acc_VoucherDetail VoucherDetails { get; set; }
    }
}