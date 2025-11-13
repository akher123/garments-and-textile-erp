
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.HRMModel
{
    public partial class HrmPenaltyType : HrmSearchModel<HrmPenaltyType>
    {
        public HrmPenaltyType()
        {
            this.HrmPenalty = new HashSet<HrmPenalty>();
        }

        public int PenaltyTypeId { get; set; }

        [Required]
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<HrmPenalty> HrmPenalty { get; set; }
    }
}
