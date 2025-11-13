using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IStorePurchaseRequisitionManager
    {

       List<Inventory_StorePurchaseRequisitionDetail> GetStorePurchaseRequisitionDetails(int storePurchaseRequisitionId);
       VStorePurchaseRequisition GetVStorePurchaseRequisitionById(int storePurchaseRequisitionId);
       Inventory_StorePurchaseRequisition GetStorePurchaseRequisitionById(int storePurchaseRequisitionId);
       int EditStorePurchaseRequisition(Inventory_StorePurchaseRequisition storePurchaseRequisition,List<Inventory_StorePurchaseRequisitionDetail>storePurchaseRequisitionDetails );


       List<VStorePurchaseRequisition> GetStorePurchaseRequisitionsByPaging(VStorePurchaseRequisition model, out int totalRecords);
       int DeletePurchaseRequisition(int storePurchaseRequisitionId);
       string GetNewRequisitionNo();
       int SaveStorePurchaseRequisition(Inventory_StorePurchaseRequisition storePurchaseRequisition);
       int DeleteDeletePurchaseRequisitionDetail(int? storePurchaseRequisitionDetailId);
       List<Inventory_StorePurchaseRequisition> GetStorePurchaseRequisitions(string searchString);
       Dictionary<string, VItemReceiveDetail> GetVStorePurchaseRequisitionDetails(int storePurchaseRequisitionId);

       int SaveStorePurchase(Inventory_StorePurchaseRequisition storePurchaseRequisition);
       List<VStorePurchaseRequisition> GetStorePurchaseByPaging(VStorePurchaseRequisition model, out int totalRecords);
    }
}
