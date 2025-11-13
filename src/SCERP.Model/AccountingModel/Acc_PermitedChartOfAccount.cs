using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public  class Acc_PermitedChartOfAccount
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public decimal ControlCode { get; set; }
        public int ControlLevel { get; set; }
        public bool IsActive { get; set; }
        public virtual Acc_CompanySector Acc_CompanySector { get; set; }
    }
}
