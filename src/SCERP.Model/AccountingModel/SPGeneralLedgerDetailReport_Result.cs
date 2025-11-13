using System;

namespace SCERP.Model.AccountingModel
{
    public partial class SPGeneralLedgerDetailReport_Result
    {
        public string SectorName { get; set; }
        public string CostCentreName { get; set; }
        public long VoucherNo { get; set; }
        public string VoucherRefNo { get; set; }
        public string GLNameSearch { get; set; }
        public string GLName { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string VoucherDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Balance { get; set; }
    }
}
