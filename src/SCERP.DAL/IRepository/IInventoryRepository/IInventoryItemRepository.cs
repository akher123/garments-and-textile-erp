using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IInventoryItemRepository : IRepository<Inventory_Item>
    {
       
        Inventory_Item GetItemById(int itemId, int branchId);
        bool CheckAnyItemExist(int itemId, int branchId);

        bool IsBranchWiseItemExist(InventorySearchField searchField);
        List<Inventory_Item> AutocompliteItemByBranch(string itemName);
        string GetItemCode(int? subGroupId, int? branchId);
        string GetMaxSubGroupCode(int groupId);
        int RateUpdate(DateTime? updateDate,int itemId);

        int MonthlyRateUpdate(DateTime? updateDate);
    }
}
