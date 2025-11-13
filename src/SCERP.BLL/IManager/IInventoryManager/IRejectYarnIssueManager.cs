using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IRejectYarnIssueManager
    {
        List<Inventory_RejectYarnIssue> GetRejectYarns
            (string searchString, int pageIndex, string sort, string sortdir, out int totalRecord);
       Inventory_RejectYarnIssue GetRejectYarnById(int rejectYarnIssueId);
        List<SpRejectYarnDetail> GetRejectYarnDetailById(int rejectYarnIssueId);
        int SaveRejectYarn(Inventory_RejectYarnIssue rejectYarnIssue);

        string GetNewId();
        int EditRejectYarn(Inventory_RejectYarnIssue rejectYarnIssue);
        DataTable GetRejectYarnReport(int rejectYarnIssueId);
    }
}
