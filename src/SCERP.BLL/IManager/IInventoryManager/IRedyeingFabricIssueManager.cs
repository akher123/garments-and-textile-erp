using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IRedyeingFabricIssueManager
    {
        List<Inventory_RedyeingFabricIssue> GetRedyeingFabIssueByPaging(string compId, string searchString, int pageIndex, out int totalRecords);
        string GetNewRefNo(string compId);
        Inventory_RedyeingFabricIssue GetRedyeingFabricIssueById(long redyeingFabricIssueId);
        List<VwRedyeingFabricIssueDetail> GetVwRedyeingFabricIssueDetailById(long redyeingFabricIssueId);
        int EditRedyeingFabricIssue(Inventory_RedyeingFabricIssue modelRedyeingFabricIssue);
        int SaveRedyeingFabricIssue(Inventory_RedyeingFabricIssue modelRedyeingFabricIssue);
        object GetRedyeingReceivedBatchAutocomplite(string compId, string searchString, long partyId);
        IEnumerable<dynamic> GetRedyeingReceivedBatchDetailByBatchId(long batchId);
        int DeleteRedyeingFabricIssue(long redyeingFabricIssueId);
        int ApproveRedyeingIssueById(long redyeingFabricIssueId);
        DataTable GetRedyeingFabricIssue(long redyeingFabricIssueId);
    }
}
