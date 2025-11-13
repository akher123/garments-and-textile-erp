using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public partial class OM_SampleStyle
    {
        public int SampleStyleId { get; set; }
        [Required]
        public string StyleRefId { get; set; }
        public int SampleOrderId { get; set; }
        public string StyleNo { get; set; }
        public string Fabrication { get; set; }
        public string ItemName { get; set; }
        public string SampleType { get; set; }
        public DateTime? SampleDate { get; set; }
        public DateTime? EFDate { get; set; }
        public string Gsm { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public double? FabQty { get; set; }
        public int? StyleQty { get; set; }
        public string Remarks { get; set; }
        public string CompId { get; set; }
        public string McDia { get; set; }
        public string FinishDia { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public string RibFab { get; set; }
        public double? RibQty { get; set; }
        public string ContasFab { get; set; }
        public double? ContasQty { get; set; }
        public virtual OM_SampleOrder OM_SampleOrder { get; set; }
    }
}
