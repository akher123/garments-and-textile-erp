using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwPartCutting
    {
        public int PartCuttingId { get; set; }
        public string CompId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string PartSL { get; set; }
        public string ComponentRefId { get; set; }
        public string ComponentName { get; set; }
    }
}
