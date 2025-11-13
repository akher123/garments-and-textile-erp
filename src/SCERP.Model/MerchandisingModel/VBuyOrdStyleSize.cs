using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VBuyOrdStyleSize
    {
        public long OrderStyleSizeId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public string SizeRefId { get; set; }
        public Nullable<int> SizeRow { get; set; }
        public string SizeName { get; set; }

    }
}
