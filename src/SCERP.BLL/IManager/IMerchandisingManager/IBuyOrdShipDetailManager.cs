using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface  IBuyOrdShipDetailManager
    {
       int SaveBuyOrdShipDetail(OM_BuyOrdShipDetail ordShipDetail, string orderStyleRefId);
       int UpdateBuyOrdShipDetail(OM_BuyOrdShipDetail ordShipDetail, string orderStyleRefId);
    }
}
