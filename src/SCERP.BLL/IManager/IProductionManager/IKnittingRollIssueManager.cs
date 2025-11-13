using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IKnittingRollIssueManager
    {
        List<PROD_KnittingRollIssue> GetKnittingRollIssueByPaging
            (int pageIndex, string orderStyleRefId, string sortdir,string searchKey, string compId, out int totalRecords);
         string GetNewRefNo(string compId,int? challanType);
        PROD_KnittingRollIssue GetKnittingRollIssueById(int knittingRollIssueId);
        int SaveKnittingRollIssue(PROD_KnittingRollIssue knittingRollIssue);
        int EditKnittingRollIssue(PROD_KnittingRollIssue knittingRollIssue);

        List<VwKnittingRollIssueDetail> GetKnittingRollsByOrderStyleRefId(string orderStyleRefId,int challanType, string compId);
        List<VwKnittingRollIssueDetail> GetRollIssueDetailsByKnittingRollIssueId(int knittingRollIssueId);
        int DeleteKnittingRollIssueById(long knittingRollIssueId);
        List<PROD_KnittingRollIssue> GetKnittingRollIssueByOrderStyleRefId(string orderStyleRefId);
        int IsReceivedRollChallan(int knittingRollIssueId, string currentUserCompId);
        object GetRollBySearchKey(string searchKey);

        List<PROD_KnittingRollIssue> GetPartyKnittingChallanList(int pageIndex, string searchString, string compId, out int totalRecords);
    }
}
