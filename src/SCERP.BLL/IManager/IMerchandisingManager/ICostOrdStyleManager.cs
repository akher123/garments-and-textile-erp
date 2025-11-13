using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface ICostOrdStyleManager
    {
        List<VCostOrderStyle> GetCostCostOrdStyle(string orderStyleRefId);
        VCostOrderStyle GetCostCostOrderById(long costOrderStyleId);
        int SaveCostOrdStyle(OM_CostOrdStyle model);
        int EditCostOrdStyle(OM_CostOrdStyle model);
        int DeleteCostOrdStyle(long costOrderStyleId);
        bool IsPreCostExit(string orderStyleRefId);

        int SaveCostOrdStyleList(List<OM_CostOrdStyle> costOrdStyles);
    }
}


