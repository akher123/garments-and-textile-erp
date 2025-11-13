namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class EmployeeBonus : SearchModel<EmployeeBonus>
    {
        public int EmployeeBonusId { get; set; }

        public System.Guid EmployeeId { get; set; }

        public int? BonusTypeId { get; set; }
        public int BonusRuleId { get; set; }
        
        [Required (ErrorMessage = "Required!")]
        public decimal Amount { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime EffectiveDate { get; set; }

        public string Remarks { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
