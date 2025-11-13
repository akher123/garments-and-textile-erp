using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IMaterialIssueManager
    {
        VMaterialIssue GetVMaterialIssueById(int materialIssueId);

        string GetNewIssueReceiveNo();
        int EditMaterialIssue(Inventory_MaterialIssue materialIssue);
        int SaveMaterialIssue(Inventory_MaterialIssue materialIssue);
        List<VMaterialIssue> GetMaterialIssuesByPaging(VMaterialIssue model, out int totalRecords);
        List<VMaterialIssueDetail> GetMaterialIssueDetails(int materialIssueId);
        int DeleteMaterialIssue(int? materialIssueId);
        int DeleteMaterialIssueDetail(int? materialIssueDetailId);

        List<Pro_Batch> GetAllBatch();

        List<VMaterialIssue> GetMaterialIssues(VMaterialIssue model, out int totalRecords);

        List<VMaterialIssue> GetGeneralIssueByPaging(VMaterialIssue model, out int totalRecords);
        List<VMaterialLoanReturn> GetMaterialLoanReturns(VMaterialLoanReturn model, out int totalRecords);
        VMaterialLoanReturn GetVMaterialLoanReturnById(int materialIssueId);
        int DeleteBatchWiseIssue(int materialIssueId,int isuueType);
        List<VLoanGiven> GetMaterialLoanGiven(VLoanGiven model, out int totalRecords);
        VLoanGiven GetVLaonGivenById(int materialIssueId);
        DataTable GetChemicalIssueChallan(int materialIssueId);
    }
}
