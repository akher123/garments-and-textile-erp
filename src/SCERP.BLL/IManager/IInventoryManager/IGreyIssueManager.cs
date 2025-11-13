using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IGreyIssueManager
    {
        List<Inventory_GreyIssue> GetGreyReceiveByPaging
            (DateTime? fromDate, DateTime? toDate, string searchString, string compId, int pageIndex, out int totalRecords);
        List<KnittingOrderDelivery> GetKnittingOrderDelivery(int programId,long greyIssueId);
        int SaveGreyIssue(Inventory_GreyIssue greyIssue);
        string GetNewRow(string compId);

        Inventory_GreyIssue GetGreyissueById(long p);
        int DeleteGreyIssue(long greyIssueId);
        int EditGreyIssue(Inventory_GreyIssue greyIssue);
        DataTable GetGeryIssuePartyChallan(long greyIssueId);

        int GreyIssureApproval(long greyIssueId);
    }
}
