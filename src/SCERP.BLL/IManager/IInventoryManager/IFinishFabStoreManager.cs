using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IFinishFabStoreManager
    {
        List<Inventory_FinishFabStore> GetFinishFabricLsitByPaging(string compId ,  string searchString, int pageIndex, out int totalRecords);
        Inventory_FinishFabStore GetFinishFabricById(long finishFabStoreId);
        string GetNewRefId(string compId);
        int EditFinishFabric(Inventory_FinishFabStore finishFabStore);
        int SaveFinishFabric(Inventory_FinishFabStore finishFabStore);
        int DeleteFinishFabric(long finishFabStoreId);
        List<SpInvFinishFabStore> GetDetailChallanBy(long dyeingSpChallanId, long finishFabStoreId);
        IEnumerable GetBatchDeailsById(long batchId);
        object GetFinishFabIssueDetail(long batchDetailId);
        DataTable GetFinishFabricDeliveryDataTable(long finishFabIssueId);
        DataTable GetFinishFabricDeliveryByStyle(string orderStyleRefId);
    }
}
