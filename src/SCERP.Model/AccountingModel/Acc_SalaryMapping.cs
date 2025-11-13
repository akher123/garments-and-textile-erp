using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class Acc_SalaryMapping
    {
        public int Id { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> CostCentreId { get; set; }
        public Nullable<int> SalaryHeadId { get; set; }
        public Nullable<int> GLID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EDT { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }

        public virtual Acc_CompanySector Acc_CompanySector { get; set; }
        public virtual Acc_CostCentre Acc_CostCentre { get; set; }
        public virtual SalaryHead SalaryHead { get; set; }
    }
}
