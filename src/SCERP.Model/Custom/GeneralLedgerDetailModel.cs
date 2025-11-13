using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class GeneralLedgerDetailModel
    {
        public string SectorName { get; set; }
        public string CostCentreName { get; set; }
        public Nullable<long> VoucherNo { get; set; }
        public string VoucherRefNo { get; set; }
        public string GLNameSearch { get; set; }
        public string GLName { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string VoucherDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> FirstCurValue { get; set; }
        public Nullable<decimal> SecendCurValue { get; set; }
        public Nullable<decimal> ThirdCurValue { get; set; }
    }
}
