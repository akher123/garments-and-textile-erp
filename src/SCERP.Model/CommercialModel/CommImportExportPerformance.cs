using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommImportExportPerformance
    {
        public int Id { get; set; }
        public Nullable<int> SerialId { get; set; }
        public Nullable<int> LcId { get; set; }
        public string LcNo { get; set; }
        public Nullable<System.DateTime> LcDate { get; set; }
        public Nullable<long> BuyerId { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<decimal> LcQuantity { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public string BbLcNo { get; set; }
        public Nullable<System.DateTime> BbLcDate { get; set; }
        public Nullable<int> SupplierCompanyRefId { get; set; }
        public Nullable<decimal> BbLcAmount { get; set; }
        public Nullable<decimal> BbLcQuantity { get; set; }
        public Nullable<System.DateTime> BbLcMatureDate { get; set; }
        public Nullable<System.DateTime> BBLcExpiryDate { get; set; }
        public Nullable<System.DateTime> BBLcExtensionDate { get; set; }
        public string BbLcIssuingBank { get; set; }
        public string BbLcIssuingBankAddress { get; set; }
        public string BBLcReceivingBank { get; set; }
        public string BBLcReceivingBankAddress { get; set; }
        public Nullable<int> BbLcType { get; set; }
        public string Beneficiary { get; set; }
        public Nullable<int> BBLcPartialShipment { get; set; }
        public string BBLcDescription { get; set; }
        public string IfdbcNo { get; set; }
        public Nullable<System.DateTime> IfdbcDate { get; set; }
        public Nullable<decimal> IfdbcValue { get; set; }
        public Nullable<int> PcsSanctionAmount { get; set; }
        public string ExportNo { get; set; }
        public Nullable<System.DateTime> ExportDate { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> InvoiceValue { get; set; }
        public string BankRefNo { get; set; }
        public Nullable<System.DateTime> BankRefDate { get; set; }
        public Nullable<decimal> RealizedValue { get; set; }
        public Nullable<System.DateTime> RealizedDate { get; set; }
        public string BillOfLadingNo { get; set; }
        public Nullable<System.DateTime> BillOfLadingDate { get; set; }
        public string SBNo { get; set; }
        public Nullable<System.DateTime> SBNoDate { get; set; }
    }
}
