
using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface ICompConsumptionManager
    {
       List<VCompConsumption> GetCompConsumption(string orderStyleRefId);
       int GetNewCompConsuotionSlNo(string orderStyleRefId);
       int SaveCompConsumption(OM_CompConsumption model);
       OM_CompConsumption GetCompConsumptionById(long compConsumptionId);
       int EditCompConsumption(OM_CompConsumption model);
       int DeleteCompConsumption(long compConsumptionId);
       List<VwCompConsumptionOrderStyle> GetVwCompConsumptionOrderStyle(OM_BuyOrdStyle omBuyOrdStyle, out int totalRecords);
       VCompConsumption GetVCompConsumptionById(long compConsumptionId);
    }
}
