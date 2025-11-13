using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.CommercialModel
{
    public partial class CommExport
    {
        public CommExport()
        {
            this.CommExportDetail = new HashSet<CommExportDetail>();
            this.CommPackingListDetail = new HashSet<CommPackingListDetail>();
        }

        public long ExportId { get; set; }
        [Required]
        public string ExportRefId { get; set; }
        [Required]
        public string ExportNo { get; set; }
        public Nullable<System.DateTime> ExportDate { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        [Required]
        public Nullable<decimal> InvoiceQuantity { get; set; }
        [Required]
        public Nullable<decimal> InvoiceValue { get; set; }
        public string BankRefNo { get; set; }
        public Nullable<System.DateTime> BankRefDate { get; set; }
        public Nullable<decimal> RealizedValue { get; set; }
        public Nullable<System.DateTime> RealizedDate { get; set; }
        public Nullable<decimal> ShortFallAmount { get; set; }
        public string ShortFallReason { get; set; }
        public string BillOfLadingNo { get; set; }
        public Nullable<System.DateTime> BillOfLadingDate { get; set; }
        public string SBNo { get; set; }
        public Nullable<System.DateTime> SBNoDate { get; set; }
        [Required]
        public int LcId { get; set; }
        public string CompId { get; set; }
        public Nullable<decimal> FcAmount { get; set; }
        public string UdNoLocal { get; set; }
        public string UdNoForeign { get; set; }
        public Nullable<System.DateTime> UdDateLocal { get; set; }
        public Nullable<System.DateTime> UdDateForeign { get; set; }
        public string PaymentMode { get; set; }
        public string IncoTerm { get; set; }
        public string ShipmentMode { get; set; }
        public string PortOfLanding { get; set; }
        public string PortOfDischarge { get; set; }
        public string FinalDestination { get; set; }
        [Required]
        public int? SalseContactId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }

        public virtual COMMLcInfo COMMLcInfo { get; set; }
        public virtual ICollection<CommExportDetail> CommExportDetail { get; set; }
        public virtual ICollection<CommPackingListDetail> CommPackingListDetail { get; set; }

    }
}
