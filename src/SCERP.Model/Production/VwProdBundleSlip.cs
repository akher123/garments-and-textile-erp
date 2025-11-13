using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
    public partial class VwProdBundleSlip
    {
        public string CompId { get; set; }
        public string JobNo { get; set; }
      
        public string CuttingBatchRefId { get; set; }
        public string BatchNo { get; set; }
        public Nullable<long> BundleNo { get; set; }
        public Nullable<int> BundleStart { get; set; }
        public Nullable<int> BundleEnd { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string BuyerName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public string Country { get; set; }
        public string ComponentRefId { get; set; }
        public string BundleCuttingId { get; set; }
        public string ComponentName { get; set; }
        
    }
}
