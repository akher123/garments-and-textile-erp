using SCERP.Model.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommercialModel
{
    public class VwCommLcStyle : ProSearchModel<VwCommLcStyle>
    {
        public int LcId { get; set; }
        public string LcNo { get; set; }
        public string OrderNo { get; set; }
        public decimal Quantity { get; set; }
    }
}
