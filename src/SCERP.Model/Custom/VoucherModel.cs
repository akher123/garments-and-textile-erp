using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Custom
{
    public class VoucherModel
    {
        public long Id { get; set; }
        public string VoucherType { get; set; }
        public long VoucherNo { get; set; }
        public string VoucherRefNo { get; set; }
        public string VoucherDate { get; set; }
        public string CheckNo { get; set; }
        public string CheckDate { get; set; }
        public string Particulars { get; set; }
        public Nullable<int> GLID { get; set; }
        public string DetailParticulars { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public string TotalAmountInWord { get; set; }
        public decimal ControlCode { get; set; }
        public decimal AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ControlName { get; set; }
        public string SectorCode { get; set; }
        public string SectorName { get; set; }
        public string CostCentreCode { get; set; }
        public string CostCentreName { get; set; }
        public string PeriodName { get; set; }
        public string PeriodStartDate { get; set; }
        public string PeriodEndDate { get; set; }
        public string Address { get; set; }
    }
}
