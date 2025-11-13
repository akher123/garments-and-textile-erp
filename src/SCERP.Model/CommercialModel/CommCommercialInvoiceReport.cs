using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class CommCommercialInvoiceReport
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyFax { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string ExportLcNo { get; set; }
        public Nullable<System.DateTime> ExportLcDate { get; set; }
        public string NegotiatingBank { get; set; }
        public string negotiatingBankAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeAddress3 { get; set; }
        public string ConsigneeBank { get; set; }
        public string ConsigneeBankAddress { get; set; }
        public string ExportNo { get; set; }
        public Nullable<System.DateTime> ExportDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public Nullable<System.DateTime> BillOfLadingDate { get; set; }
        public string PaymentMode { get; set; }
        public string IncoTerm { get; set; }
        public string ShipmentMode { get; set; }
        public string PortOfLanding { get; set; }
        public string PortOfDischarge { get; set; }
        public string FinalDestination { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string RefNo { get; set; }
        public string ItemDescription { get; set; }
        public string ExportCode { get; set; }
        public Nullable<int> CartonQuantity { get; set; }
        public Nullable<int> ItemQuantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string StyleName { get; set; }
        public string AmountInWords { get; set; }
    }
}