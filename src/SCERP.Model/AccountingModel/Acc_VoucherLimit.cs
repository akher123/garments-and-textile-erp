
namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Acc_VoucherLimit : BaseModel
    {
        public int Id { get; set; }
        public string VoucherType { get; set; }
        public Nullable<decimal> VoucherLimit { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
