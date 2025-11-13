using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
   public class VwCommLcInfo
    {
        public int LcId { get; set; }
        public string LcNo { get; set; }
        public Nullable<System.DateTime> LcDate { get; set; }
        public Nullable<long> BuyerId { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<decimal> LcQuantity { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public string BTMACertificate { get; set; }
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> ReceivingBankId { get; set; }
        public string SalesContactNo { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> AppliedDate { get; set; }
        public Nullable<double> IncentiveClaimValue { get; set; }
        public string NewMarketCliam { get; set; }
        public string BkmeaCertificat { get; set; }
        public string FirstAuditStatus { get; set; }
        public string CertificateOvservation { get; set; }
        public Nullable<double> FinalClaimAmount { get; set; }
        public Nullable<double> ReceiveAmount { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public string CashIncentiveRemarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string RStatus { get; set; }
        public Nullable<double> CommissionPrc { get; set; }
        public Nullable<double> CommissionsAmount { get; set; }
        public Nullable<double> FreightAmount { get; set; }
        public double PcAmount { get; set; }
        public Nullable<double> ScQty { get; set; }
        public Nullable<double> ScAmnt { get; set; }
        public decimal BbLcAmount { get; set; }
        public decimal BbLcQuantity { get; set; }
        public string BuyerName { get; set; }
        public string RcvBank { get; set; }
        public string UdEoNo { get; set; }
        public string FileNo { get; set; }
        public int? GroupLcId { get; set; }
    }
}
