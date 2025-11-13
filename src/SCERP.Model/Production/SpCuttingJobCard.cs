using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
   public class SpCuttingJobCard
    {
       public int SizeRow { get; set; }
       public string SizeName { get; set; }
       public string OrderStyleRefId { get; set; }
       public string SizeRefId { get; set; }
       public int Quantity { get; set; }
       public int CuttingQuantity { get; set; }
       public int? Ratio{ get; set; }
    }
}
