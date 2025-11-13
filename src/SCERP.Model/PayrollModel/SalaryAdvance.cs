namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SalaryAdvance : SearchModel<SalaryAdvance>
    {
        public int SalaryAdvanceId { get; set; }
        public System.Guid EmployeeId { get; set; }

        [Range(1, 1000000.00, ErrorMessage = "Required!")]
        public decimal Amount { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }

        [Required (ErrorMessage = "Required!")]
        public decimal Percentage { get; set; }


        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime ReceivedDate { get; set; }

        public string Remarks { get; set; }
    }
}
