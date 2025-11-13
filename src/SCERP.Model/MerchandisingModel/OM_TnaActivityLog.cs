using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class OM_TnaActivityLog
    {
        public long ActivityLogId { get; set; }
        public long TnaId { get; set; }
        public DateTime EditedDate { get; set; }
        public Guid EditedBy { get; set; }
        public string ValueText { get; set; }
        public string KeyName { get; set; }
    }
}
