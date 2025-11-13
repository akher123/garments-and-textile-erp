using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
   
    
    public partial class PROD_CuttingProcessStyleActive
    {
        public long CuttingProcessStyleActiveId { get; set; }
        public string ProcessRefId { get; set; }
        public string CompId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string BuyerRefId { get; set; }
         [Required(ErrorMessage = "Required")]
        public string OrderNo { get; set; }
         [Required(ErrorMessage = "Required")]
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = "Required")]
        public DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
