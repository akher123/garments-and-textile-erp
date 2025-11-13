using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommLcWithOrderDetailReport
    {
        public int LcId { get; set; }
        public string BuyerName { get; set; }
        public string LcNo { get; set; }
        public Nullable<System.DateTime> LcDate { get; set; }
        public Nullable<decimal> LcAmount { get; set; }
        public Nullable<decimal> LcQuantity { get; set; }
        public Nullable<System.DateTime> MatureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExtensionDate { get; set; }
        public string MerchandiserName { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public string Fabrication { get; set; }
        public Nullable<long> BuyerOrderId { get; set; }
        public string CompId { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string SampleOrdNo { get; set; }
        public string BuyerRefId { get; set; }
        public string DGRefNo { get; set; }
        public string Closed { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> Style { get; set; }
        public string StyleRefId { get; set; }
    }
}
