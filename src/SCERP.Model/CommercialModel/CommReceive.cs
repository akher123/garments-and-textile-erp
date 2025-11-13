using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public partial class CommReceive : ProSearchModel<COMMLcInfo>
    {
        public CommReceive()
        {
            this.CommReceiveDetail = new HashSet<CommReceiveDetail>();
        }

        public int ReceiveId { get; set; }
        public string ReceiveRefNo { get; set; }
        public Nullable<int> BbLcId { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ReceiveDate { get; set; }

        public string PassBookPageNo { get; set; }
        public string MushakChallanNo { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MushakChallanDate { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<CommReceiveDetail> CommReceiveDetail { get; set; }
    }
}
