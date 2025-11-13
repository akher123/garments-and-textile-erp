using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
    public partial class VwCuttingSequence
    {
        public long CuttingSequenceId { get; set; }
        public string CuttingSequenceRefId { get; set; }
        public string CompId { get; set; }
        public string ComponentRefId { get; set; }
        public int SlNo { get; set; }
        public long ComponentId { get; set; }
        public string ComponentName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string BuyerRefId { get; set; }
          [Required(ErrorMessage = "Required")]
        public string OrderNo { get; set; }
          [Required(ErrorMessage = "Required")]
        public string OrderStyleRefId { get; set; }
          [Required(ErrorMessage = "Required")]
        public string ColorRefId { get; set; }

        public string ColorName  { get; set; }

    }
}
