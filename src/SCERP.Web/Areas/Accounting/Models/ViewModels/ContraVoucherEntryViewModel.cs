using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;


namespace SCERP.Web.Areas.Accounting.Models.ViewModels
{
    public class ContraVoucherEntryViewModel : Acc_VoucherMaster
    {
        public Acc_VoucherDetail VoucherDetails { get; set; }
        public string Balance { get; set; }
    }
}