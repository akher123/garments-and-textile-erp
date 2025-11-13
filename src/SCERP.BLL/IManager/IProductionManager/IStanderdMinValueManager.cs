using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface IStanderdMinValueManager
    {
       List<PROD_StanderdMinValue> GetStanderdMinValuesByStyleOrder(string orderStyleRefId);
       string GetStanderdMinValueRefId();
       List<PROD_SubProcess> GetSubProcessList();
       int SaveSmv(PROD_StanderdMinValue standerdMinValue);
       PROD_StanderdMinValue GetSmvById(long standerdMinValueId);
       int EditSmv(PROD_StanderdMinValue standerdMinValue);
       bool IsSmvCreated(string orderStyleRefId);
       int DeleteStanderdMinValue(long standerdMinValueId);
       Dictionary<string, VwStanderdMinValDetail> GetVwSmvDetails(long standerdMinValueId);
    }
}
