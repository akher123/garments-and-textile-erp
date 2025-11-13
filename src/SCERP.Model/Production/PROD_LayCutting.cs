using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
    public partial class PROD_LayCutting
    {
        public long LayCuttingId { get; set; }
        public string CompId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string LaySl { get; set; }
        [Required(ErrorMessage = @"Required")]
        public string SizeRefId { get; set; }
        
        public Nullable<int> Ratio { get; set; }
        public virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
    }
}
