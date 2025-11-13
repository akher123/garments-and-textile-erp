using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IRedyeingFabricIssueRepository:IRepository<Inventory_RedyeingFabricIssue>
    {
        List<VwRedyeingFabricIssueDetail> GetVwRedyeingFabricIssueDetailById(long redyeingFabricIssueId);
        object GetRedyeingReceivedBatchAutocomplite(string compId, string searchString, long partyId);
        IEnumerable<dynamic> GetRedyeingReceivedBatchDetailByBatchId(long batchId);
    }
}
