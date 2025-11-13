using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.DAL.IRepository.IProductionRepository
{
    public interface ICuttingProcessStyleActiveRepository : IRepository<PROD_CuttingProcessStyleActive>
    {
        List<VwCuttingProcessStyleActive> GetCuttingProcessStyleActiveByPaging(string compId, string buyerRefId, string orderNo, string orderStyleRefId);
        IEnumerable GetOrderByBuyer(string compId, string buyerRefId);
        IEnumerable GetStyleByOrderNo(string compId, string orderNo);
        VwCuttingProcessStyleActive GetVwStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId);
    }
}
