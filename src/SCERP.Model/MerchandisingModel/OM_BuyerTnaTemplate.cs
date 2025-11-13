using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public partial class OM_BuyerTnaTemplate
    {
        public int TemplateId { get; set; }
        public string CompId { get; set; }
        public string BuyerRefId { get; set; }
        public int TemplateTypeId { get; set; }
        public int ActivityId { get; set; }
        public double Duration { get; set; }
        public string Remarks { get; set; }
        public int SerialNo { get; set; }
        public int? RSerialNo { get; set; }
        public double? FDuration { get; set; }
        public string RType { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    }
}
