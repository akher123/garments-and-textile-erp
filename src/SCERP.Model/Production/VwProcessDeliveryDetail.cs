using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class VwProcessDeliveryDetail
    {
        public long ProcessDeliveryDetailId { get; set; }
        public long ProcessDeliveryId { get; set; }
        public string CompId { get; set; }
        public long CuttingTagId { get; set; }
        public long CuttingBatchId { get; set; }
        public string ComponentRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
       public string CuttingBatchRefId { get; set; }
       public string SizeName { get; set; }
       public string ColorName { get; set; }
    }
}
