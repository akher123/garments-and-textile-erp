using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommGetLcWithoutOrderReport
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
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public string BuyerName { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
