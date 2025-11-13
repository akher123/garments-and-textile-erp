using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
   public interface ICuttingBatchRepository:IRepository<PROD_CuttingBatch>
    {
       IQueryable<VwCuttingBatch> GetAllVwCuttingBatches(Expression<Func<VwCuttingBatch, bool>> predicates);
       List<SpCuttingJobCard> GetCuttingJobCards(string orederStyleRefId, string colorRefId, string componentRefId, string compId, string orderShipRefId);


       List<VwCuttingApproval> GetCuttingApproval
           (string compId, string buyerRefId, string orderNo, string orderStyleRefId, string colorRefId, string componentRefId, string approvalStatus);
       IQueryable   <VwCuttingBatch> GetAllCuttingBatchListByPaging(DateTime? cuttingDate, string searchString,int?matchineId);
    }
}
