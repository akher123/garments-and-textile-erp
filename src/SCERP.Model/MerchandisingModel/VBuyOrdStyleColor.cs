using System;

namespace SCERP.Model.MerchandisingModel
{
   public class VBuyOrdStyleColor
   {
       public long OrderStyleColorId { get; set; }
       public string CompId { get; set; }
       public string OrderStyleRefId { get; set; }
       public string ColorRefId { get; set; }
       public Nullable<int> ColorRow { get; set; }
       public string ColorName { get; set; }
    }
}
