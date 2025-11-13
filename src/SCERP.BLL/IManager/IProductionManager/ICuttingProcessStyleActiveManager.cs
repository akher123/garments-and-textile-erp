using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
   public interface ICuttingProcessStyleActiveManager
    {
       int SaveCuttingProcessStyleActive(PROD_CuttingProcessStyleActive model);
       List<VwCuttingProcessStyleActive> GetCuttingProcessStyleActiveByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string buyerRefId, string orderNo, string orderStyleRefId);
       PROD_CuttingProcessStyleActive GetStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId);
       int EditCuttingProcessStyleActive(PROD_CuttingProcessStyleActive model);
       bool IsCuttingProcessStyleActiveExist(PROD_CuttingProcessStyleActive model);
       int DeleteCuttingProcessStyleActive(string orderStyleRefId);
       IEnumerable GetOrderByBuyer(string buyerRefId);
       IEnumerable GetStyleByOrderNo(string orderNo);
       VwCuttingProcessStyleActive GetVwStyleInCuttingByCuttingProcessStyleActiveId(long cuttingProcessStyleActiveId);
    }
}
