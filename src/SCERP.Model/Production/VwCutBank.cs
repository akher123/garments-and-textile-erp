using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
    
    
    public partial class VwCutBank
    {
        public long CutBankId { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string ComponentRefId { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<int> CutFQty { get; set; }
        public Nullable<int> BankQty { get; set; }
        public Nullable<int> BalanceQty { get; set; }
        public string ComponentType { get; set; }
        public Nullable<int> SolidQty { get; set; }
        public Nullable<int> PrintRcvQty { get; set; }
        public Nullable<int> EmbRcvQty { get; set; }
        public Nullable<int> PrintRejQty { get; set; }
        public Nullable<int> EmbRejQty { get; set; }
        public Nullable<int> FabricRejQty { get; set; }
        public string BuyerName { get; set; }
        public string OrderRefId { get; set; }
        public string StyleName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ComponentName { get; set; }
    }
}
