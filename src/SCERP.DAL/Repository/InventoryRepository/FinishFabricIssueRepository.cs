using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class FinishFabricIssueRepository : Repository<Inventory_FinishFabricIssue>, IFinishFabricIssueRepository
    {
        public FinishFabricIssueRepository(SCERPDBContext context) : base(context)
        {
        }

      
    }
}
