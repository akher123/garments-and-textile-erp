using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IAdvanceMaterialIssueManager
    {
       List<VwAdvanceMaterialIssue> GetAdvanceMaterialIssue
           (int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string searchString, int storeId, out int totalRecords);

       string GetNewRefId(int storeId);
       Inventory_AdvanceMaterialIssue GetAdvanceMaterialIssueById
           (long advanceMaterialIssueId);

       int SaveAdvanceMaterialIssue(Inventory_AdvanceMaterialIssue materialIssue);
       List<VwAdvanceMaterialIssueDetail> GetVwAdvanceMaterialssDtl(long advanceMaterialIssueId);
       VwAdvanceMaterialIssue GetVwAdvanceMaterialIssueById(long advanceMaterialIssueId);
       List<VwAdvanceMaterialIssue> GetAdvanceMaterialIssues(DateTime? fromDate, DateTime? toDate,string searchString, int storeId);
       int DeleteAdvanceMaterialIssue(long advanceMaterialIssueId, int iType);
       string GetAccNewRefId();
       int LoackYarnIssue(long advanceMaterialIssueId);
       IEnumerable GetDeliverdYarn(string yarndyeing,int storeId);
       Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetDeliverdYarnDetail
           (string iRefId, string compId);

       Inventory_AdvanceMaterialIssue GetYarnDeliveryByRefd(string piBookingRefId, string compId);
        Dictionary<string, VwAdvanceMaterialIssueDetail> GetAccessoriesRcvSummary(string orderStyleRefId, string currentUserCompId);
        Dictionary<string, VwAdvanceMaterialIssueDetail> GetAccessorisEditRcvDetails(long materialIssueAdvanceMaterialIssueId);
        DataTable GetAccessoriesIssueChallanDataTable(long advanceMaterialIssueId);
        DataTable GetAccessoriesIssueDetailStatus(DateTime? modelFromDate, DateTime? modelToDate, string currentUserCompId);
        List<Inventory_AdvanceMaterialIssue> GeChallanListByPartyId(int partyId);
        DataTable GetDeliverdYarnByStyle(string orderStyleRefId);
        List<ProgramYarnWithdrow> GetProgramYarnWithdrow(string programRefId);
    }
}
