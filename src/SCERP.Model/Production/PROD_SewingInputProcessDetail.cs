using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{


    public partial class PROD_SewingInputProcessDetail
    {
        public long SewingInputProcessDetailId { get; set; }
        public long SewingInputProcessId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SizeRefId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int InputQuantity { get; set; }
        public string CompId { get; set; }

        public virtual PROD_SewingInputProcess PROD_SewingInputProcess { get; set; }
    }
}
