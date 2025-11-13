using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.CommercialModel
{
    public class CommLcToOrderReport : ProSearchModel<CommLcToOrderReport>
    {
        public int LcId { get; set; }
        public string LcNo { get; set; }
        public string BuyerName { get; set; }
        public Nullable<System.DateTime> LcDate { get; set; }
        public Nullable<decimal> LcQuantity { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public string LcIssuingBank { get; set; }
        public string LcIssuingBankAddress { get; set; }
        public string ReceivingBank { get; set; }
        public string ReceivingBankAddress { get; set; }
        public Nullable<int> LcType { get; set; }
        public string Beneficary { get; set; }
        public string Description { get; set; }
        public Nullable<long> BuyerOrderId { get; set; }
        public string CompId { get; set; }
        public string OrderNo { get; set; }
        public string RefNo { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<decimal> OAmount { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
    }
}
