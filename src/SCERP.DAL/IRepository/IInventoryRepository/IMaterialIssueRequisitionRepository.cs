using System;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IMaterialIssueRequisitionRepository : IRepository<Inventory_MaterialIssueRequisition>
    {
        IQueryable<VMaterialIssueRequisition> GetMaterialIssueRequisitionByPaging(Expression<Func<VMaterialIssueRequisition, bool>> predicate);
        VMaterialIssueRequisition GetVMaterialIssueRequisitionById(int materialIssueRequisitionId);
    }
}
