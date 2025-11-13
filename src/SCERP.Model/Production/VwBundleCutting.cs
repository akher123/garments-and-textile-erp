using System;

namespace SCERP.Model.Production
{
   public class VwBundleCutting
    {
        public long BundleCuttingId { get; set; }
        public string CompId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string BundleRef { get; set; }
        public string BundleNo { get; set; }
        public string RollNo { get; set; }
        public string SizeRefId { get; set; }
        public Nullable<int> XSC { get; set; }
        public string ColorRefId { get; set; }
        public string ComponentRefId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> BundleStart { get; set; }
        public Nullable<int> BundleEnd { get; set; }
        public string PSL { get; set; }
        public string SSL { get; set; }
        public string BatchNo { get; set; }
        public string Combo { get; set; }
        public string RollName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ComponentName { get; set; }
    }
}
