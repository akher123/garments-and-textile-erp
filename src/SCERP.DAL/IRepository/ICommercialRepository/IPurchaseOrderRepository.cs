using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.CommonModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.ICommercialRepository
{
    public interface IPurchaseOrderRepository:IRepository<CommPurchaseOrder>
    {
        List<VwPurchaseOrderDetail> GetConsumptionItems(string companyId, string orderStyleRefId, int supplierId, string accessoriesGroupCode);
        List<Mrc_SupplierCompany> GetAssignedSuppliers(string orderStyleRefId, string compId);
        List<VwCommPurchaseOrder> GetPurchaseOrderList(string companyId, string pType, string orderStyleRefId, string orderNo);
        IEnumerable<VwPurchaseOrderDetail> GetAllRateQuittedYarns
            (string companyId, string orderStyleRefId, int supplierId, string accessoriesGroupCode);

        List<Mrc_SupplierCompany> GetQuitedYarnSupplier(string orderStyleRefId, string compId);
        List<MaterialStatus> GetSyleWiseMaterialStatus(string orderStyleRefId, string compId);
        List<CommPurchaseOrder> GetAllPurchaseOrders();
        CommPurchaseOrder GetPurchaseOrderByPurchaseOrderNo(string purchaseOrderNo);
        List<CommPurchaseOrderDetail> GetPurchaseOrderDetails(string purchaseOrderNo);
        List<VwCommPurchaseOrder> GetApprovalPurchaseOrderList(string companyId,bool isApproved, string pType, string searchString);
        List<VwCommPurchaseOrder> GetPurchaseOrders(string companyId, bool isApproved, string pType, string searchString, string buyerRefId, string orderNo, String orderStyleRefId);
        List<ProFormaInvoice> GetApporovedBookingBySupplier(int supplierId, string compId);
    }
}
