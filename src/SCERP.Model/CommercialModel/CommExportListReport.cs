using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommExportListReport : ProSearchModel<CommExportListReport>
    {
        public long ExportId { get; set; }
        public string LcNo { get; set; }
        public string ExportRefId { get; set; }
        public string ExportNo { get; set; }
        public string ExportDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public Nullable<decimal> InvoiceValue { get; set; }
        public string BankRefNo { get; set; }
        public string BankRefDate { get; set; }
        public Nullable<decimal> RealizedValue { get; set; }
        public string RealizedDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public string BillOfLadingDate { get; set; }
        public string SBNo { get; set; }
        public string SBNoDate { get; set; }
        public int LcId { get; set; }
        public string CompId { get; set; }
        public decimal? FcAmount { get; set; }
        public string UdNoLocal { get; set; }
        public string UdNoForeign { get; set; }
        public string UdDateLocal { get; set; }
        public string UdDateForeign { get; set; }
    }
}
