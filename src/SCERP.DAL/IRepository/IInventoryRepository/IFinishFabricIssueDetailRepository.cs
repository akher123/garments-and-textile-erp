using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IFinishFabricIssueDetailRepository:IRepository<Inventory_FinishFabricIssueDetail>
    {
        List<VwFinishFabricIssueDetail> GetFinishFabIssureDetails(long finishFabIssueId);

        object GetReceivedBatchAutocomplite(string compId, string searchString,long partyId);
        IEnumerable GetReceivedBatchDetailByBatchId(long batchId);
        object GetReceivedBatchAutoComplite(string searchString);
        bool IsExistReceivedBatch(string batchNo);
    }
}
