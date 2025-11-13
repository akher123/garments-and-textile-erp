using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IRejectYarnIssueRepository:IRepository<Inventory_RejectYarnIssue>
    {
        List<SpRejectYarnDetail> GetRejectYarnDetailById(int rejectYarnIssueId);
    }
}
