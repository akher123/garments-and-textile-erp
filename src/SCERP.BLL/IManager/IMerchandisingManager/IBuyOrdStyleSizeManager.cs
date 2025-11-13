using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IBuyOrdStyleSizeManager
    {
        List<VBuyOrdStyleSize> GetBuyOrdStyleSize(string orderStyleRefId);
        int SaveBuyOrdStyleSize(OM_BuyOrdStyleSize size);
        int DelteBuyOrdStyleSize(OM_BuyOrdStyleSize size);
        int EditBuyOrdStyleSize(OM_BuyOrdStyleSize size);
        int EditBuyOrdStyleColor(OM_BuyOrdStyleColor color);
        int SaveBuyOrdStyleColor(OM_BuyOrdStyleColor color);
        List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId);


        int DelteBuyOrdStyleColor(OM_BuyOrdStyleColor color);
        VBuyOrdStyleSize GetBuyOrdStyleSizeById(long id);
    }
}
