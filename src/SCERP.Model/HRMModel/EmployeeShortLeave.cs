using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public partial class EmployeeShortLeave : SearchModel<EmployeeShortLeave>
    {
        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public byte ReasonType { get; set; }

        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ReasonDescription { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public System.DateTime Date { get; set; }
        public System.TimeSpan FromTime { get; set; }
        public System.TimeSpan ToTime { get; set; }
        public Nullable<System.TimeSpan> TotalHours { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
