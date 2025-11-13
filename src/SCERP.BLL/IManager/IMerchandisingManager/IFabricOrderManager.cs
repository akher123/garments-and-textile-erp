using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IFabricOrderManager
    {
        List<VwFabricOrder> GetFabricOrderByPaging(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string status,string searchString, out int totalRecords);
        OM_FabricOrder GetFabricOrderbyId(int fabricOrderId);
        string GetFabricOrderRefId();
        int EditFabricOrder(OM_FabricOrder fabricOrder);
        int SaveFabricOrder(OM_FabricOrder fabricOrder);
        int DeleteFabricOrder(int fabricOrderId);
        List<VwCompConsumptionOrderStyle> GeFabricConsStyleList(string orderNo);
        List<VwCompConsumptionOrderStyle> GeFabricOrderDetail(int fabricOrderId);
        int UpdateFabricOrderStatus(string status, int fabricOrderId);
        List<VwFabricOrderDetail> GetVwFabricOrders(int pageIndex, string buyerRefId, string orderNo, string orderStyleRefId, out int totalRecords);
        List<VwFabricOrderDetail> GetVwFabricOrders(int pageIndex, string searchString, out int totalRecords);
        List<VwFabricOrder> GetApprovedFabricOrders
            
            (int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate, string status, string searchString, out int totalRecords);
        int FabricBookingLock(string orderStyleRefId, string compId);
        bool IsFabricBookingLock(string orderStyleRefId, string compId);
        bool IsFabricBookingLock(int fabricOrderId);
    }
}
