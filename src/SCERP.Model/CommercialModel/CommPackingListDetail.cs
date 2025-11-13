using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommPackingListDetail
    {
        public int PackingListId { get; set; }
        public Nullable<long> ExportId { get; set; }
        public Nullable<int> Block { get; set; }
        public string CountryName { get; set; }
        public string OrderStyleRefId { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public Nullable<int> CartonQuantity { get; set; }
        public Nullable<int> CartonCapacity { get; set; }
        public Nullable<int> CartonFrom { get; set; }
        public Nullable<int> CartonTo { get; set; }
        public string ContainerNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual CommExport CommExport { get; set; }
    }
}
