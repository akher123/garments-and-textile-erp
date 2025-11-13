using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class VwDyeingJobOrderDetail
    {
        public long DyeingJobOrderDetailId { get; set; }
        public long DyeingJobOrderId { get; set; }
        public int ItemId { get; set; }
      
        public string ItemName { get; set; }
         
        public double? Gsm { get; set; }
    
        public string ComponentRefId { get; set; }
      
        public string ComponentName { get; set; }
     
        public string ColorRefId { get; set; }
     
        public string ColorName { get; set; }
    
        public string MdSizeRefId { get; set; }
        public string MdName { get; set; }
      
        public string FdSizeRefId { get; set; }
      
        public string FdName { get; set; }
    
        public double Quantity { get; set; }
  
        public double Rate { get; set; }
        public string CompId { get; set; }
        public string Remarks { get; set; }
        public double? GreyWit { get; set; }
    }
}
