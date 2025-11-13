using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
  public class CommPackingCredit
    {
        public int PackingCreditId { get; set; }
        [Required]
        [Range(1, Double.MaxValue)]
        public int LcId { get; set; }
        [Required]
        public System.DateTime CreditDate { get; set; }
        [Required]
        [Range(1, Double.MaxValue)]
        public double Amount { get; set; }
        public double? UsdAmount { get; set; }
      
        public bool IsAcive { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }

        public virtual COMMLcInfo COMMLcInfo { get; set; }
    }
}
