using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_CuttingGradding
    {
        public long CuttingGraddingId { get; set; }
        public long CuttingBatchId { get; set; }
        public string FromSizeRefId { get; set; }
        public string ToSizeRefId { get; set; }
        public int Quantity { get; set; }
        public string CompId { get; set; }
        public virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
    }
}
