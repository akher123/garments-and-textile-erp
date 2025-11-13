using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IInventoryManager
{
    public interface IInventoryItemManager
    {
     
        ResponsModel SaveInventoryItem(Inventory_Item inventoryItem);
        Inventory_Item GetItemById(int itemId);
        bool CheckAnyItemExist(int itemId, int branchId);
        ResponsModel EditInventoryItem(Inventory_Item item);
        List<Inventory_Item> AutocompliteItem(string itemName, int subgroupId);
        List<Inventory_Item> AutocompliteItemByBranch(string itemName);
        string GetItemCode(int? subGroupId, int? branchId);
        string GetMaxSubGroupCode(int groupId);

        IEnumerable<Inventory_Group> GetChartOfItemTreeView(string searchKey);

        Inventory_Group GetGroupById(int groupid);
        int RateUpdate(DateTime? updateDate,int itemId);
        int MonthlyRateUpdate(DateTime? updateDate);
    }
}
