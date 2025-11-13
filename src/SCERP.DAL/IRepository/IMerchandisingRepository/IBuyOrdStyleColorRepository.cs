using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
   public interface IBuyOrdStyleColorRepository:IRepository<OM_BuyOrdStyleColor>
    {
       List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId,string compId);
       IEnumerable GetSizeByOrderStyleRefId(string orderStyleRefId, string compId);
        VBuyOrdStyleSize GetBuyOrdStyleSizeById(long id);
        VBuyOrdStyleColor GetBuyOrdStyleColorById(long id);
        int UpdateBuyOrdStyleColor(long orderStyleColorId,string colorRefId);
    }
}
