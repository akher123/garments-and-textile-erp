using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
   public class DailyFabricReceiveRepository:Repository<PROD_DailyFabricReceive>,IDailyFabricReceiveRepository
    {
       public DailyFabricReceiveRepository(SCERPDBContext context) : base(context)
       {
       }

       public List<VwReceivedFabricProductionSummary> GetVwReceivedFabricProductionSummary(string compId, int pageIndex, string searchString, string buyerRefId, DateTime? fromDate,
           DateTime? toDate, out int totalRecords)
       {
           int pageSize = AppConfig.PageSize;
           var vwReceivedFabricProductionSummaries= Context.VwReceivedFabricProductionSummaries.Where(x => x.CompId == compId
               &&(x.BuyerRefId==buyerRefId||String.IsNullOrEmpty(buyerRefId))
               &&(x.StyleName.Trim().Replace(" ","").ToLower().Contains(searchString.Trim().Replace(" ","").ToLower())||String.IsNullOrEmpty(searchString)));
           totalRecords = vwReceivedFabricProductionSummaries.Count();
           vwReceivedFabricProductionSummaries = vwReceivedFabricProductionSummaries
                            .OrderByDescending(r => r.OrderStyleRefId)
                           .Skip(pageIndex * pageSize)
                           .Take(pageSize);
           return vwReceivedFabricProductionSummaries.ToList();
       }

       public VwReceivedFabricProductionSummary GetDailyFabricReceive(string styleName, string orderNo, string orderStyleRefId,
           string consRefId, string componentRefId, string colorRefId)
       {
           string compId = PortalContext.CurrentUser.CompId;
          return Context.VwReceivedFabricProductionSummaries.FirstOrDefault( x =>x.CompId==compId&&x.StyleName == styleName && x.OrderNo == orderNo && x.OrderStyleRefId == orderStyleRefId &&x.ComponentRefId == componentRefId && x.ConsRefId == consRefId&&x.ColorRefId==colorRefId);
       }

       public List<SpProdDailyFabricReceive> GetDailyFabricReceived(string compId, DateTime receivedDate, string buyerRefId,string serchString)
       {
           SqlParameter pramCompId = new SqlParameter("CompId", compId);
           SqlParameter pramReceivedDate = new SqlParameter("ReceivedDate", receivedDate);
           SqlParameter pramBuyerRefId = new SqlParameter("BuyerRefId", buyerRefId??"-1");
           SqlParameter pramStyleName = new SqlParameter("StyleName", serchString??"" );
           return Context.Database.SqlQuery<SpProdDailyFabricReceive>("SpProdDailyFabricReceive @CompId,@ReceivedDate,@BuyerRefId ,@StyleName", new object[] { pramCompId, pramReceivedDate, pramBuyerRefId, pramStyleName }).ToList();
      
       }
    }
}
