using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IMaterialIssueRepository:IRepository<Inventory_MaterialIssue>
    {
        VMaterialIssue GetVMaterialIssueById(int materialIssueId);
        string GetNewIssueReceiveNo();

        IQueryable<VMaterialIssue> GetMaterialIssuesByPaging(Expression<Func<VMaterialIssue, bool>> predicate);
        IQueryable<VMaterialLoanReturn> GetMaterialLoanReturns(Expression<Func<VMaterialLoanReturn, bool>> predicate);

        VLoanGiven GetVLaonGivenById(int materialIssueId);
        IQueryable<VLoanGiven> GetMaterialLoanGiven(VLoanGiven model, out int totalRecords);
        DataTable GetChemicalIssueChallan(int materialIssueId);
        DataTable GetLoanReturnChallan(int materialIssueId);
    }
}
