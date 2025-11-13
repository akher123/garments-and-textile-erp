using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
    
    public partial class PROD_RejectAdjustment
    {
        public long RejectAdjustmentId { get; set; }
        public long CuttingBatchId { get; set; }
        public int RejectQty { get; set; }
        public string SizeRefId { get; set; }
        public string CompId { get; set; }
        public virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
    }
}
