using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.CommonModel;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IPurchaseOrderManager
    {
     
        CommPurchaseOrder GetPurchaseOrderById(long purchaseOrderId);
        int DeletePurchseOrder(long purchaseOrderId);

        IEnumerable<VwCommPurchaseOrder> GetPurchaseOrderList(string pType, string orderStyleRefId, string orderNo);
        IEnumerable<VwPurchaseOrderDetail> GetAllAccessories(string orderStyleRefId, int supplierId);
        string GetNewPurchaseOrderRefId(string pType );
        int SavePurchaseOrder(CommPurchaseOrder purchaseOrder);
        IEnumerable<VwPurchaseOrderDetail> GetAllPurchaseOrderDetails(long purchaseOrderId);
        int EditPurchaseOrder(CommPurchaseOrder purchaseOrder);
        List<Mrc_SupplierCompany> GetAssignedSuppliers(string orderStyleRefId);
        IEnumerable<VwPurchaseOrderDetail> GetAllRateQuittedYarns(string orderStyleRefId, int supplierId,string PiRefId);
        List<Mrc_SupplierCompany> GetQuitedYarnSupplier(string orderStyleRefId);
        List<MaterialStatus> GetSyleWiseMaterialStatus(string orderStyleRefId);
        List<CommPurchaseOrder> GetAllPurchaseOrders();
        DataTable GetYarBookingSummaryByStyle(string orderStyleRefId);
        CommPurchaseOrder GetPurchaseOrderByPurchaseOrderNo(string purchaseOrderNo);
        List<CommPurchaseOrderDetail> GetPurchaseOrderDetails(string purchaseOrderNo);
        IEnumerable GetPurchaseOrderByType(string pType, string orderStyleRefId);
        Inventory_Booking GetPurchaseOrderByRefId(string piBookingRefId);
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetAllPurchaseOrderDetailsByRefId(string piBookingRefId);
        IEnumerable<VwCommPurchaseOrder> GetApprovalPurchaseOrderList(string pType, bool isApproved, string searchString, string buyerRefId, string orderNo, String orderStyleRefId);
        bool IsBookingApproed(long purchaseOrderId);
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetYarnPurchaseOrderDetails(long piBookingRefId);
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetAllPurchaseOrderDetailsByStyleRefId(string orderStyleRefId);
        Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetYarnPurchaseOrderDetailsByPiRefId(string piRefId);
        List<ProFormaInvoice> GetApporovedBookingBySupplier(int supplierId, string compId);
    }
}
