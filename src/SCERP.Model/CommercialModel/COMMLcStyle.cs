using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class COMMLcStyle : ProSearchModel<COMMLcStyle>
    {
        public int LcStyleId { get; set; }
        public Nullable<int> LcRefId { get; set; }
        public string OrderNo { get; set; }
        public string OrderStyleRefId { get; set; }
        public Nullable<decimal> StyleQuantity { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
