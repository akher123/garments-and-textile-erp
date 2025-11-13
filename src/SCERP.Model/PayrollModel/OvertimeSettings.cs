


using System;
using SCERP.Model;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model
{

    public partial class OvertimeSettings : SearchModel<OvertimeSettings>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = @"Required")]
        public decimal? OvertimeHours { get; set; }

        [Required(ErrorMessage = @"Required")]
        [DataType(DataType.Text)]
        public decimal? OvertimeRate { get; set; }

        [Required(ErrorMessage = @"Required")]
        [DataType(DataType.Date)]
        public System.DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
