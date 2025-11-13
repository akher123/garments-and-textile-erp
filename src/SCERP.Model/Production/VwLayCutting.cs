using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class VwLayCutting
    {
        public long LayCuttingId { get; set; }
        public string CompId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string LaySl { get; set; }
     
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public Nullable<int> Ratio { get; set; }

    }
}
