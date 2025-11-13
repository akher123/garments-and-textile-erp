using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IDailyFabricReceiveRepository : IRepository<PROD_DailyFabricReceive>
    {
        List<VwReceivedFabricProductionSummary> GetVwReceivedFabricProductionSummary
            (string compId,int pageIndex, string searchString, string buyerRefId, DateTime? fromDate, DateTime? toDate, out int totalRecords);

        VwReceivedFabricProductionSummary GetDailyFabricReceive
            (string styleName, string orderNo, string orderStyleRefId, string consRefId, string componentRefId, string colorRefId);

        List<SpProdDailyFabricReceive> GetDailyFabricReceived
            (string compId, DateTime receivedDate, string buyerRefId, string serchString);
    }
}
