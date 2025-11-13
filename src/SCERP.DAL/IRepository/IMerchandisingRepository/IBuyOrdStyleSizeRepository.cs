using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IBuyOrdStyleSizeRepository:IRepository<OM_BuyOrdStyleSize>
    {
        List<VBuyOrdStyleSize> GetBuyOrdStyleSize(string orderStyleRefId,string compId);
        int UpdateBuyOrdStyleSize(long orderStyleSizeId, string sizeRefId);
    }
}
