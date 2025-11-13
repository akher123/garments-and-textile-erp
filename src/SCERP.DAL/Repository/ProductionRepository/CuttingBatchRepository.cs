using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CuttingBatchRepository : Repository<PROD_CuttingBatch>, ICuttingBatchRepository
    {
        public CuttingBatchRepository(SCERPDBContext context) : base(context)
        {
        }
        public IQueryable<VwCuttingBatch> GetAllVwCuttingBatches(Expression<Func<VwCuttingBatch, bool>> predicates)
        {
            return Context.vwCuttingBatches.Where(predicates);
        }

        public List<SpCuttingJobCard> GetCuttingJobCards(string orederStyleRefId, string colorRefId, string componentRefId, string compId, string orderShipRefId)
        {
             SqlParameter prderStyleRefIdSp = new SqlParameter("@OrderStyleRefId", orederStyleRefId);
             SqlParameter colorRefIdSp = new SqlParameter("@ColorRefId", colorRefId);
             SqlParameter componentRefIdSp = new SqlParameter("@ComponentRefId", componentRefId);
             SqlParameter copIdSp = new SqlParameter("@CompId", compId);
             SqlParameter orderShipRefIdSp = new SqlParameter("@OrderShipRefId", orderShipRefId??"-1");
             var spList = new[] { prderStyleRefIdSp, colorRefIdSp, componentRefIdSp, copIdSp, orderShipRefIdSp };
            return
                Context.Database.SqlQuery<SpCuttingJobCard>(
                    "SpCuttingJobCard @OrderStyleRefId,@ColorRefId,@ComponentRefId,@CompId,@OrderShipRefId", spList).ToList();
        }

        public List<VwCuttingApproval> GetCuttingApproval(string compId, string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId,
            string componentRefId, string approvalStatus)
        {
            return
                Context.VwCuttingApprovals.Where(
                    x =>
                        x.CompId == compId && (x.BuyerRefId == buyerRefId || buyerRefId == null) && (x.OrderRefId == orderNo || orderNo == null) && (x.OrderStyleRefId == orderStyleRefId || orderStyleRefId == null) && (x.ColorRefId == colorRefId || colorRefId == null) &&
                        (x.ComponentRefId == componentRefId || componentRefId == null) && x.ApprovalStatus == approvalStatus).OrderByDescending(x => x.CuttingDate).ToList();
        }

        public IQueryable<VwCuttingBatch> GetAllCuttingBatchListByPaging(DateTime? cuttingDate, string searchString, int? machineId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            return
                 Context.vwCuttingBatches.Where(
                     x => x.CompId == compId && (x.MachineId == machineId||(machineId==null))
                       &&(x.CuttingDate == cuttingDate)
                         && (x.CuttingBatchRefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)));
        }
    }
}
