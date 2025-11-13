using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PROD_FinishingProcessDetail
    {
        public long FinishingProcessDetailId { get; set; }
        public long FinishingProcessId { get; set; }
        public string SizeRefId { get; set; }
        public int InputQuantity { get; set; }
        public string CompId { get; set; }

        public virtual PROD_FinishingProcess PROD_FinishingProcess { get; set; }
    }
}
