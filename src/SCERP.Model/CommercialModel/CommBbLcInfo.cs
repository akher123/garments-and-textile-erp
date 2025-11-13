using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.CommercialModel
{
    public partial class CommBbLcInfo : ProSearchModel<CommBbLcInfo>
    {
        public CommBbLcInfo()
        {
            this.CommBbLcItemDetails = new HashSet<CommBbLcItemDetails>();
        }

        public int BbLcId { get; set; }
        public int LcRefId { get; set; }
        public string BbLcNo { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BbLcDate { get; set; }

        public Nullable<int> SupplierCompanyRefId { get; set; }
        public Nullable<decimal> BbLcAmount { get; set; }
        public Nullable<decimal> BbLcQuantity { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MatureDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ExtensionDate { get; set; }

        public string BbLcIssuingBank { get; set; }
        public string BbLcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> IssuingBankId { get; set; }
        [Required]
        public Nullable<int> BbLcType { get; set; }
        public string Beneficiary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public string IfdbcNo { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> IfdbcDate { get; set; }

        public Nullable<decimal> IfdbcValue { get; set; }
        public Nullable<int> PcsSanctionAmount { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PaymentDate { get; set; }

        public string BtmeaNo { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BtmeaDate { get; set; }

        public string BeNo { get; set; }
     

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BeDate { get; set; }

        public Nullable<decimal> Vat { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public string CompId { get; set; }

        public Nullable<int> ItemType { get; set; }
        public string Supplier { get; set; } //add new for report view

        public int? SalseContactId { get; set; }
        public DateTime? MaturityDateFrom { get; set; }
        public DateTime? MaturityDateTo { get; set; }
        public DateTime? ExpiryDateFrom { get; set; }
        public DateTime? ExpiryDateTo { get; set; }
        public bool? DonothaveMaturityDate { get; set; }
        public bool? DonothaveExpiryDate { get; set; }


        public virtual COMMLcInfo COMMLcInfo { get; set; }
        public virtual Mrc_SupplierCompany Mrc_SupplierCompany { get; set; }

        public virtual ICollection<CommBbLcItemDetails> CommBbLcItemDetails { get; set; }
    }
}
