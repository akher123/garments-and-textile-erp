using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.Model.Planning
{
    public partial class PLAN_Activity : ProSearchModel<PLAN_TNA>
    {
        public PLAN_Activity()
        {
            this.PLAN_TNA = new HashSet<PLAN_TNA>();
        }

        public int Id { get; set; }
        public string CompId { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public string ActivityMode { get; set; }
        public string StartField { get; set; }
        public string EndField { get; set; }
        public Nullable<int> BufferDay { get; set; }
        public Nullable<bool> IsRelative { get; set; }
        public Nullable<int> SerialId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }      

        public virtual ICollection<PLAN_TNA> PLAN_TNA { get; set; }
    }
}
