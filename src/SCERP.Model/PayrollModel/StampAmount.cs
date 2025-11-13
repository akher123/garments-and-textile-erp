using SCERP.Common;

namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class StampAmount : SearchModel<StampAmount>
    {
        public int StampAmountId { get; set; }
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public Nullable<System.DateTime> FromDate { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ToDate { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
