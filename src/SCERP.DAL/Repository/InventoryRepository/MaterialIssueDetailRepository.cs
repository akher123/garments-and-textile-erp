using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialIssueDetailRepository :Repository<Inventory_MaterialIssueDetail>, IMaterialIssueDetailRepository
    {
        public MaterialIssueDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VMaterialIssueDetail> GetMaterialIssueDetails(int materialIssueId)
        {
            return Context.VMaterialIssueDetails.Where(x => x.MaterialIssueId == materialIssueId).ToList();
        }
    }
}
