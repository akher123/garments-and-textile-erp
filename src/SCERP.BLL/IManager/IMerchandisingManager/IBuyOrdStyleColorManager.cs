using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IBuyOrdStyleColorManager
    {
        List<VBuyOrdStyleColor> GetBuyOrdStyleColor(string orderStyleRefId);
        VBuyOrdStyleColor GetBuyOrdStyleColorById(long id);
    }
}
