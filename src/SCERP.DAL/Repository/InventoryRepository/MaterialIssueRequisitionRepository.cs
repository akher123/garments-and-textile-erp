using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialIssueRequisitionRepository :Repository<Inventory_MaterialIssueRequisition>, IMaterialIssueRequisitionRepository
    {
        public MaterialIssueRequisitionRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<VMaterialIssueRequisition> GetMaterialIssueRequisitionByPaging(Expression<Func<VMaterialIssueRequisition, bool>> predicate)
        {
            return Context.VMaterialIssueRequisitions.Where(predicate);
        }

        public VMaterialIssueRequisition GetVMaterialIssueRequisitionById(int materialIssueRequisitionId)
        {
            return
                Context.VMaterialIssueRequisitions.SingleOrDefault(
                    x => x.MaterialIssueRequisitionId == materialIssueRequisitionId);
        }
    }
}
