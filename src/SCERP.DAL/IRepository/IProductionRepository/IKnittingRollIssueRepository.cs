using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface IKnittingRollIssueRepository:IRepository<PROD_KnittingRollIssue>
    {
        List<VwKnittingRollIssueDetail> GetKnittingRollsByOrderStyleRefId(string programRefId, int challanType, string compId);
        List<VwKnittingRollIssueDetail> GetRollIssueDetailsByKnittingRollIssueId(int knittingRollIssueId);
    }
}
