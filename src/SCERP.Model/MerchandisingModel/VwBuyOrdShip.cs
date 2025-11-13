using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MerchandisingModel
{
    public class VwBuyOrdShip : OM_BuyOrdShip
    {
        public string CountryName { get; set; }
        public string PortOfLoadingName { get; set; }
    }
}
