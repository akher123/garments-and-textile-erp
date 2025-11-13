

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model
{
    using System;
    using System.Collections.Generic;

    public partial class SMSInformation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
