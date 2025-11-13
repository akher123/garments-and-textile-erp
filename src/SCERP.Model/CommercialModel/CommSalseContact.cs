using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommSalseContact
    {

        public int SalseContactId { get; set; }
        public int LcId { get; set; }
        [Required]
        public string LcNo { get; set; }
        [Required]
        public System.DateTime LcDate { get; set; }
        [Required]
        public Nullable<long> BuyerId { get; set; }
        [Required]
        [Range(1, Double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        [Range(1, Double.MaxValue)]
        public double Quantity { get; set; }
        public DateTime? AuditDate { get; set; }
        public double? AuditAmount { get; set; }
        public DateTime? CashIncentiveDate { get; set; }
        public double? CashIncentiveAmount { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBankAddress { get; set; }
        [Required]
        public Nullable<int> ReceivingBankId { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual COMMLcInfo COMMLcInfo { get; set; }
    }
}
