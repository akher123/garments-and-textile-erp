using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class COMMLcInfo : ProSearchModel<COMMLcInfo>
    {
        public COMMLcInfo()
        {
            CommExport = new HashSet<CommExport>();
            CommBbLcInfo = new HashSet<CommBbLcInfo>();
            CommPackingCredit = new HashSet<CommPackingCredit>();
            CommSalseContact = new HashSet<CommSalseContact>();
        }

        public int LcId { get; set; }
        public string SI { get; set; }
        [Required]
        public string LcNo { get; set; }
        [Required]

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> LcDate { get; set; }
        [Required]
        public Nullable<long> BuyerId { get; set; }
        public int BankId { get; set; }
        [Required]
        public Nullable<decimal> LcAmount { get; set; }
        [Required]
        public Nullable<decimal> LcQuantity { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MatureDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ExtensionDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ShipmentDate { get; set; }

        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> ReceivingBankId { get; set; }
        [Required]
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string SalesContactNo { get; set; }
        public string Description { get; set; }
        [Required]
        public double? CommissionPrc { get; set; }
        [Required]
        public double? CommissionsAmount { get; set; }
        [Required]
        public Nullable<double> FreightAmount { get; set; }

        //Cash Intensive Statuse Start
        public Nullable<System.DateTime> AppliedDate { get; set; }
        public Nullable<double> IncentiveClaimValue { get; set; }
        public string NewMarketCliam { get; set; }
        public string BTMACertificate { get; set; }
        public string BkmeaCertificat { get; set; }
        public string FirstAuditStatus { get; set; }
        public string CertificateOvservation { get; set; }
        public Nullable<double> FinalClaimAmount { get; set; }
        public Nullable<double> ReceiveAmount { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public string CashIncentiveRemarks { get; set; }

        public string CompleteStatus { get; set; }
        //Cash Intensive Statuse End
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string RStatus { get; set; }
        public string UdEoNo { get; set; }
        public string FileNo { get; set; }
        public int? GroupLcId { get; set; }
        public ICollection<CommExport> CommExport { get; set; }
        public ICollection<CommBbLcInfo> CommBbLcInfo { get; set; }
        public ICollection<CommPackingCredit> CommPackingCredit { get; set; }
        public ICollection<CommSalseContact> CommSalseContact { get; set; }

    }
}
