using System.Collections.Generic;
using SCERP.Model.CommercialModel;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IPurchaseOrderDetailRepository:IRepository<CommPurchaseOrderDetail>
    {
        IEnumerable<VwPurchaseOrderDetail>  GetAllPurchaseOrderDetails(long purchaseOrderId, string compId);
        List<VwSpPoSheet> GetVwSpPoSheetDetails(long purchaseOrderId);
        List<VwSpPoSheet> GetVwSpPoSheetDetailByStyle(string orderStyleRefId);
        List<VwSpPoSheet> GetYarnPurchaseOrderDetailsByPiRefId(string piRefId, string compId);
    }
}
