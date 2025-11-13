using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialIssueRequisitionDetailRepository : Repository<Inventory_MaterialIssueRequisitionDetail>, IMaterialIssueRequisitionDetailRepository
    {
        public MaterialIssueRequisitionDetailRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
