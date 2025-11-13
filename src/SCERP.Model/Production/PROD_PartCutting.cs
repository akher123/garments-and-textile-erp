using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
    public partial class PROD_PartCutting
    {
        public int PartCuttingId { get; set; }
        public string CompId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string PartSL { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ComponentRefId { get; set; }
        public virtual PROD_CuttingBatch PROD_CuttingBatch { get; set; }
    }
}
