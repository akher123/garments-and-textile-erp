using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.HRMModel
{
    public partial class HrmPenalty : HrmSearchModel<HrmPenalty>
    {
        public int PenaltyId { get; set; }
        
        public Guid EmployeeId { get; set; }

        [Required]
        public string EmployeeCardId { get; set; }

        [Required]
        public int PenaltyTypeId { get; set; }

        [Required]
        public decimal Penalty { get; set; }

        [Required]
        public System.DateTime PenaltyDate { get; set; }

        public string Reason { get; set; }

        [Required]
        public Guid ClaimerId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? EditedDate { get; set; }

        public Guid? EditedBy { get; set; }

        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual HrmPenaltyType HrmPenaltyType { get; set; }
    }
}
