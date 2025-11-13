using System;
using System.Collections.Generic;

namespace SCERP.Model.Production
{
  
    public partial class VwCuttingTag
    {
        public long CuttingTagId { get; set; }
        public long CuttingSequenceId { get; set; }
        public string CompId { get; set; }
        public bool IsSolid { get; set; }
        public bool IsPrint { get; set; }
        public bool IsEmbroidery { get; set; }
        public string CuttingSequenceRefId { get; set; }
        public string BuyerRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public int SlNo { get; set; }
        public string ComponentRefId { get; set; }
        public string ComponentName { get; set; }
        public string ColorRefId { get; set; }
        public string ColorName { get; set; }
        public long CuttingTagSupplierId { get; set; }
        public string SupplierId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string CompanyName { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public int EmblishmentStatus { get; set; }
        public double? Rate { get; set; }
        public double? DeductionAllowance { get; set; }
    }
}
