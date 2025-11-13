using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IAdvanceMaterialIssueDetailRepository:IRepository<Inventory_AdvanceMaterialIssueDetail>
   {
       List<VwAdvanceMaterialIssueDetail> GetVwAdvanceMaterialssDtl(long advanceMaterialIssueId, string compId);

       List<VwAdvanceMaterialIssueDetail> GetDeliverdYarnDetail(string refId, string compId);
       List<AccessoriesReceiveBalance> GetAccessoriesRcvSummary(string orderStyleRefId, string compId);

       List<AccessoriesReceiveBalance> GetAccessorisEditRcvDetails(long materialIssueAdvanceMaterialIssueId);
       DataTable GetAccessoriesIssueChallanDataTable(long advanceMaterialIssueId);
       DataTable GetAccessoriesIssueDetailStatus(DateTime? fromDate, DateTime? toDate, string compid);
   }
}
