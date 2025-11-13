using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IFinishFabricIssueManager
    {
        string GetFinishFabIssureRefId(string compId);
        Inventory_FinishFabricIssue GetFinishFabIssureById(long finishFabIssueId);
        List<VwFinishFabricIssueDetail> GetFinishFabIssureDetails(long finishFabIssueId);
        int SaveFinishFabricIssue(Inventory_FinishFabricIssue finishFabricIssue);
        int EditFinishFabricIssue(Inventory_FinishFabricIssue finishFabricIssue);
        List<Inventory_FinishFabricIssue> GetFinishFabIssuresByPaging
            (int pageIndex, string sort, string sortdir,bool? isApproved,bool? isReceived, DateTime? toDate, DateTime? fromDate, string searchString, string compId, int? partyId, out int totalRecords);
        DataTable GetDyingBillInvoices(DateTime? invoiceDate);
        int ApprovedFabricDeliveryChallan(long finishFabIssueId);
        int DeleteFinishFabricIssue(long finishFabIssueId);
        object GetReceivedBatchAutocomplite(string currentUserCompId, string searchString,long partyId);
        IEnumerable GetReceivedBatchDetailByBatchId(long batchId);
        int UpdateFabricIssue(Inventory_FinishFabricIssue finishFabricIssue);
        object ReceivedBatchAutoComplite(string searchString);
        bool IsExistReceivedBatch(string batchNo);
    }
}
