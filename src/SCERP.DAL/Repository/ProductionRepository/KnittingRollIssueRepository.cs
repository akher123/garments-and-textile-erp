using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class KnittingRollIssueRepository : Repository<PROD_KnittingRollIssue>, IKnittingRollIssueRepository
    {
        public KnittingRollIssueRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public List<VwKnittingRollIssueDetail> GetKnittingRollsByOrderStyleRefId(string programRefId,int challanType, string compId)
        {
            bool isRejected = false;
            if (challanType == 3)
            {
                isRejected = true;
            }
            string sqlQuery = String.Format(@"select KR.*  from VwKnittingRoll as KR 
                       
                      where KR.CompId='{0}' and KR.ProgramRefId='{1}' and ISNULL(KR.IsRejected,0)='{2}' and KR.KnittingRollId not in(select KnittingRollId from PROD_KnittingRollIssueDetail )
                           order by KR.RollDate", compId, programRefId, isRejected);
            return Context.Database.SqlQuery<VwKnittingRollIssueDetail>(sqlQuery).ToList();
        }

        public List<VwKnittingRollIssueDetail> GetRollIssueDetailsByKnittingRollIssueId(int knittingRollIssueId)
        {
            string sqlQuery = String.Format(@"select KR.*,KID.KnittingRollIssueDetailId,KID.KnittingRollIssueId,KID.RollQty,(select Count(*)from PROD_KnittingRollIssueDetail where KnittingRollId=KR.KnittingRollId and KnittingRollIssueId='{0}') as CountIssue  from VwKnittingRoll as KR 
                      left join PROD_KnittingRollIssueDetail as KID on KR.KnittingRollId=KID.KnittingRollId
                      where KID.KnittingRollIssueId='{0}'  order by KR.RollDate", knittingRollIssueId);
            return Context.Database.SqlQuery<VwKnittingRollIssueDetail>(sqlQuery).ToList();
        }
    }
}
