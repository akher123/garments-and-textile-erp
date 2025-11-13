using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VwStyleFollowupStatus
    {
        public string CompId { get; set; }
        public string Merchandiser { get; set; }
        public string MerchandiserId { get; set; }
        public string BuyerRefId { get; set; }
        public string BuyerName { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public string RefNo { get; set; }
        public string StyleName { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> OrdQty { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<bool> IsAssort { get; set; }
        public Nullable<bool> IsAccsCons { get; set; }
        public Nullable<bool> IsFabCons { get; set; }
        public Nullable<decimal> FabConsQty { get; set; }
        public Nullable<bool> IsYarnCons { get; set; }
        public Nullable<bool> IsProcsSeq { get; set; }
        public Nullable<bool> IsProdProg { get; set; }
        public Nullable<bool> IsKIn { get; set; }
        public Nullable<bool> IsKOut { get; set; }
        public Nullable<bool> IsDIn { get; set; }
        public Nullable<bool> IsDOut { get; set; }
        public Nullable<bool> IsCIn { get; set; }
        public Nullable<bool> IsCOut { get; set; }
        public Nullable<bool> IsSIn { get; set; }
        public Nullable<bool> IsSOut { get; set; }
        public Nullable<bool> IsFIn { get; set; }
        public Nullable<bool> IsFOut { get; set; }
        public int ShipQty { get; set; }
    }
}
