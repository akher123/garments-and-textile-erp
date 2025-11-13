using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface IYarnConsumptionManager
    {
       List<VYarnConsumption> GetVYarnConsumptions(string consRefId,string grColorRefId);
       string GetNewYCRef();

       int SaveYarnConsumption(OM_YarnConsumption model,decimal tQty);
       int DeleteYarnCons(long yarnConsumptionId);

       List<VYarnConsumption> GetVYarnConsByOrderSyleRefId(string orderStyleRefId);
       int UpdateYarnConsRate(List<VYarnConsumption> yarnConsumptions);
       VYarnConsumption GetYarnConsumptionById(long yarnConsumptionId);
       int EditYarnConsumption(OM_YarnConsumption model, decimal tQty);
        List<VYarnConsumption> GetYarnConsSummaryByOrderSyleRefId(string orderStyleRefId);
    }
}
