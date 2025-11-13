using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface ITargetProductionManager
    {
        List<VwTargetProduction> GetTargetProductionList(string compId,string buyerRefId,string orderNo,string orderStyleRefId);
        string GetnewTargetProductionRefId(string compId);
        PLAN_TargetProduction GetTargetProductionById(long targetProductionId);

   
        int SaveTargetProduction(PLAN_TargetProduction planTargetProduction);
        int DeleteTargetProduction(long targetProductionId);


        List<VwTargetProduction> GetActiveTargetProductionList(int lineId,string compId);
        List<VwTargetProduction> GetMontylyActiveTargetProductionList(int monthId, int yearId, string compId);
    }
}
