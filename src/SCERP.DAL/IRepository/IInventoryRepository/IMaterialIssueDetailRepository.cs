using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IMaterialIssueDetailRepository:IRepository<Inventory_MaterialIssueDetail>
    {
        List<VMaterialIssueDetail> GetMaterialIssueDetails(int materialIssueId);
    }
}
