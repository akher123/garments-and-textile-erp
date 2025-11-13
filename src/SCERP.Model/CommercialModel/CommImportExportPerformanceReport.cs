using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommImportExportPerformanceReport
    {
        public int SerialId { get; set; }
        public int LcId { get; set; }
        public string LcNo { get; set; }
        public string LcDate { get; set; }
        public Nullable<long> BuyerId { get; set; }
        public string BuyerName { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<int> LcQuantity { get; set; }
        public string MatureDate { get; set; }
        public string ExpiryDate { get; set; }
        public string ExtensionDate { get; set; }
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public string BbLcNo { get; set; }
        public string BbLcDate { get; set; }
        public Nullable<int> SupplierCompanyRefId { get; set; }
        public string SupplierCompany { get; set; }
        public Nullable<decimal> BbLcAmount { get; set; }
        public Nullable<decimal> BbLcQuantity { get; set; }
        public string BBLCMatureDate { get; set; }
        public string BBLCExpiryDate { get; set; }
        public string BBLCExtensionDate { get; set; }
        public string BbLcIssuingBank { get; set; }
        public string BbLcIssuingBankAddress { get; set; }
        public string BBLCReceivingBank { get; set; }
        public string BBLCReceivingBankAddress { get; set; }
        public Nullable<int> BbLcType { get; set; }
        public string Beneficiary { get; set; }
        public Nullable<int> BBLCPartialShipment { get; set; }
        public string BBLCDescription { get; set; }
        public string IfdbcNo { get; set; }
        public string IfdbcDate { get; set; }
        public Nullable<decimal> IfdbcValue { get; set; }
        public Nullable<int> PcsSanctionAmount { get; set; }
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
    }
}
