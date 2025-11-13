using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
    
    
    public partial class VwProdBatchDetail
    {
        public long BatchDetailId { get; set; }
        public string CompId { get; set; }
        public long BatchId { get; set; }
        public int ItemId { get; set; }
        public string ComponentRefId { get; set; }
        public string MdiaSizeRefId { get; set; }
        public string FdiaSizeRefId { get; set; }
        public string GSM { get; set; }
        public double Quantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> Rate { get; set; }
        public string ItemName { get; set; }
        public string ComponentName { get; set; }
        public string FdiaSizeName { get; set; }
        public string MdiaSizeName { get; set; }
        public Nullable<double> RollQty { get; set; }
        public Nullable<double> FLength { get; set; }
        public Nullable<double> StLength { get; set; }
    }
}
