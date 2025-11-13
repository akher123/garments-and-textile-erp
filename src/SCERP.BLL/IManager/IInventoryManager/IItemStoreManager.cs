using System.Collections.Generic;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IItemStoreManager
    {
       List<VInventoryItemStore> GetInventoryItemStoreBypaging(out int totalRecords, Inventory_ItemStore model);
       int SaveItemStor(Inventory_ItemStore model, List<int> inventoryItemStoreDetailIds);
      Inventory_ItemStore GetInventoryItemStoreByItemStoreId(long itemStoreId);
       int EditItemStor(Inventory_ItemStore itemStore);
       int DeleteItemStore(long itemStoreId);
       bool CheckQcPassed(long itemStoreId);
       List<VItemReceiveDetail> GetVItemReciveDetails(long itemStoreId);
       object AutocompliteReceiveLoanInfo(string searchString);
    }
}
