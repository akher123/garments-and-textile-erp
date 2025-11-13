using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
    public partial class PLAN_ResponsiblePerson
    {
        public PLAN_ResponsiblePerson()
        {
            this.PLAN_ProcessTemplate = new HashSet<PLAN_ProcessTemplate>();
            this.PLAN_TNA = new HashSet<PLAN_TNA>();
        }

        public int Id { get; set; }
        public System.Guid ResponsiblePersonId { get; set; }
        public string ResponsiblePersonName { get; set; }
        public string ResponsiblePersonDisplayName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<PLAN_ProcessTemplate> PLAN_ProcessTemplate { get; set; }
        public virtual ICollection<PLAN_TNA> PLAN_TNA { get; set; }
    }
}
