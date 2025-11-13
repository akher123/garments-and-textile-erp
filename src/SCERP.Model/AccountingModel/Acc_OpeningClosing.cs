using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class Acc_OpeningClosing : BaseModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Sector is required")] 
        public Nullable<int> SectorId { get; set; }

        [Required(ErrorMessage = "Fiscal Period is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Fiscal Period")]
        public Nullable<int> FpId { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Account Name")]
        public Nullable<int> GlId { get; set; } 
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public virtual Acc_CompanySector Acc_CompanySector { get; set; }
        public virtual Acc_FinancialPeriod Acc_FinancialPeriod { get; set; }
    }
}
