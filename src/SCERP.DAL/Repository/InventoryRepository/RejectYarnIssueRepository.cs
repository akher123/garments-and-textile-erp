
using System.Collections.Generic;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class RejectYarnIssueRepository :Repository<Inventory_RejectYarnIssue>, IRejectYarnIssueRepository
    {
        public RejectYarnIssueRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<SpRejectYarnDetail> GetRejectYarnDetailById(int rejectYarnIssueId)
        {
           List<SpRejectYarnDetail>rjQtyList= Context.Database.SqlQuery<SpRejectYarnDetail>(string.Format("exec spGetRejectYarn {0}", rejectYarnIssueId))
                .ToList();
            return rjQtyList.ToList();
        }
    }
}
