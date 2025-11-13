using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IConsumptionManager
    {
        List<VConsumption> GetConsumptions(string orderStyleRefId,string consGroup);
        int SaveConsumption(OM_Consumption model);
        string GetNewConsRefId();
    

        VConsumption GetVConsumptionById(long consumptionId);
        int EditConsumption(OM_Consumption model);
        List<VConsumption> GetConsumptionsFabric(string orderStyleRefId);
        int UpdateConstumptionCost(List<OM_Consumption> consumptions);

        List<VConsumption> GetConsumptionList(string orderStyleRefId);

        int DeleteConsumption(string consRefId, long consumptionId);
        List<VwConsuptionOrderStyle> GetVwConsuptionOrderStyle(OM_BuyOrdStyle omBuyOrdStyle, out int totalRecords);
        VConsumption GetFabricNameByConsRefId(string consRefId);
    }
}
