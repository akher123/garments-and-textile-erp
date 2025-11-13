using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommExportDetail
    {
        public int ExportDetailId { get; set; }
        public long ExportId { get; set; }
        public string OrderStyleRefId { get; set; }
        public Nullable<int> CartonQuantity { get; set; }
        public Nullable<int> ItemQuantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string ItemDescription { get; set; }
        public string ExportCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual CommExport CommExport { get; set; }
    }
}
