using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Model.CommonModel;

namespace SCERP.Model.Production
{
    
    
    public partial class PROD_CuttingTagSupplier
    {
        public long CuttingTagSupplierId { get; set; }
        public long CuttingTagId { get; set; }
        public string CompId { get; set; }
        public long PartyId { get; set; }
        [Required]
        public int EmblishmentStatus { get; set; }
        [Required]
        public double? Rate { get; set; }
        public double? DeductionAllowance { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public virtual Party Party { get; set; }
        public virtual PROD_CuttingTag PROD_CuttingTag { get; set; }
    }
}
