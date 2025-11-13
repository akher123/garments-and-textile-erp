using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class CommBbLcItemDetails
    {
        public int BbLcItemDetailsId { get; set; }
        public int BbLcId { get; set; }
        public string Item { get; set; }

        public string Specification { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Rate { get; set; }
        public string Remarks { get; set; }

        public virtual CommBbLcInfo CommBbLcInfo { get; set; }
    }
}
