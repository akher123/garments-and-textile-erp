using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IDailyFabricReceiveManager
    {
        List<SpProdDailyFabricReceive> GetVwReceivedFabricProductionSummary
            (string searchString, string buyerRefId,  DateTime receivedDate);

        VwReceivedFabricProductionSummary GetDailyFabricReceive
            (string styleName, string orderNo, string orderStyleRefId, string consRefId, string componentRefId, string colorRefId);
        int SaveDailyFabricReceive(PROD_DailyFabricReceive dailyFabricReceive);
        int EditDailyFabricReceive(PROD_DailyFabricReceive dailyFabricReceive);
        List<PROD_DailyFabricReceive> GetDailyFabricReceiveList(string orderStyleRefId, string consRefId, string componentRefId, string colorRefId);

        PROD_DailyFabricReceive GetDailyFabricReceiveById(long fabricReceiveId);
        PROD_DailyFabricReceive GetDailyFabricReceiveByTodayDate
            (DateTime receivedDate, string orderStyleRefId, string componentRefId, string consRefId, string colorRefId);
    }
}
