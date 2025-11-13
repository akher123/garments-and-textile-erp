using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommBbLcReport
    {
        public int BbLcId { get; set; }
        public int LcRefId { get; set; }
        public string BbLcNo { get; set; }
        public Nullable<System.DateTime> BbLcDate { get; set; }
        public Nullable<int> SupplierCompanyRefId { get; set; }
        public Nullable<decimal> BbLcAmount { get; set; }
        public Nullable<decimal> BbLcQuantity { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public string BbLcIssuingBank { get; set; }
        public string BbLcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> BbLcType { get; set; }
        public string Beneficiary { get; set; }
        public Nullable<int> PartialShipment { get; set; }
        public string Description { get; set; }
        public string IfdbcNo { get; set; }
        public Nullable<System.DateTime> IfdbcDate { get; set; }
        public Nullable<decimal> IfdbcValue { get; set; }
        public Nullable<int> PcsSanctionAmount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string CompId { get; set; }
        public string Supplier { get; set; }
    }
}
