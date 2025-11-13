using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class InventoryItemManager : IInventoryItemManager
    {
        private readonly IInventoryItemRepository _itemRepository = null;
        private readonly InventoryGroupRepository _groupRepository;
        private readonly InventorySubGroupRepository _subGroupRepository;
        private readonly ResponsModel _responsModel;
        private readonly IStoreLedgerRepository _storeLedgerRepository;
        public InventoryItemManager(SCERPDBContext context)
        {
            _itemRepository = new InventoryItemRepository(context);
            _groupRepository = new InventoryGroupRepository(context);
            _subGroupRepository = new InventorySubGroupRepository(context);
            _responsModel = new ResponsModel();
            _storeLedgerRepository = new StoreLedgerRepository(context);
        }

        public ResponsModel SaveInventoryItem(Inventory_Item inventoryItem)
        {
            var _edit = 0;
            _responsModel.Message = "";
            bool isItemNameIsExist =
                _itemRepository.Exists(
                    x =>
                         x.SubGroupId == inventoryItem.SubGroupId &&
                        x.ItemName.Replace(" ", "").ToLower().Equals(inventoryItem.ItemName.Replace(" ", "").ToLower()));
            if (isItemNameIsExist)
            {
                _responsModel.Message += String.Format("{0}" + " Item name already exist",
                    inventoryItem.ItemName);
            }
            else
            {
                inventoryItem.IsActive = true;
                inventoryItem.CreatedBy = PortalContext.CurrentUser.UserId;
                inventoryItem.CreatedDate = DateTime.Now;
                inventoryItem.CompId = PortalContext.CurrentUser.CompId;
                _responsModel.Status = _itemRepository.Save(inventoryItem) > 0;
            }

            if (_edit > 0)
            {
                _responsModel.Status = true;
            }
        
            return _responsModel;
        }

        public Inventory_Item GetItemById(int itemId)
        {
            return _itemRepository.FindOne(x => x.ItemId == itemId);
        }

        public bool CheckAnyItemExist(int itemId, int branchId)
        {
            return _itemRepository.CheckAnyItemExist(itemId, branchId);
        }

        public ResponsModel EditInventoryItem(Inventory_Item item)
        {
            item.CompId = PortalContext.CurrentUser.CompId;
            int edit = 0;
            _responsModel.Message = "";
            bool isItemNameIsExist =
                _itemRepository.Exists(
                    x =>
                        x.ItemId != item.ItemId && x.SubGroupId == item.SubGroupId &&
                        x.ItemName.Replace(" ", "").ToLower().Equals(item.ItemName.Replace(" ", "").ToLower()));
            if (isItemNameIsExist)
            {
                _responsModel.Message += String.Format("{0}" + " Item name already exist",
                    item.ItemName);
            }
            else
            {
                 item.IsActive = true;
                item.EditedBy = PortalContext.CurrentUser.UserId;
                item.EditedDate = DateTime.Now;
                item.CompId = PortalContext.CurrentUser.CompId;
                 edit = _itemRepository.Edit(item);
            }
         
            if (edit > 0)
            {
                _responsModel.Status = true;
            }
            return _responsModel;

        }

        public List<Inventory_Item> AutocompliteItem(string itemName, int subgroupId)
        {
            return
                _itemRepository.Filter(
                    x =>
                        x.ItemName.Replace(" ", "").ToLower().Contains(itemName.Replace(" ", "").ToLower()) &&
                        x.SubGroupId == subgroupId).OrderBy(x => x.ItemCode).ToList();
        }

        public List<Inventory_Item> AutocompliteItemByBranch(string itemName)
        {
            return _itemRepository.AutocompliteItemByBranch(itemName);
        }

        

        public string GetItemCode(int? subGroupId, int? branchId)
        {

            var itemCode = "";
            var exists = _itemRepository.Exists(x => x.SubGroupId == subGroupId);
            if (exists)
            {
                itemCode= _itemRepository.GetItemCode(subGroupId, branchId); 
            }
            else
            {
                itemCode=_subGroupRepository.FindOne(x => x.SubGroupId == subGroupId).SubGroupCode + "001";
            }

            return itemCode;

        }

        public string GetMaxSubGroupCode(int groupId)
        {
            var subgroupCode = "";
            var exists = _subGroupRepository.Exists(x => x.GroupId == groupId);
            if (exists)
            {
                subgroupCode=_itemRepository.GetMaxSubGroupCode(groupId);
            }
            else
            {
               subgroupCode= _groupRepository.FindOne(x => x.GroupId == groupId).GroupCode +"001";
            }

            return subgroupCode;
        }


        public IEnumerable<Inventory_Group> GetChartOfItemTreeView(string searchKey)
        {
            var inventoryGroups = _groupRepository.Filter(x => x.IsActive).ToList();
            foreach (var group in inventoryGroups)
            {
                group.Inventory_SubGroup = GetAllSubGroups(group.GroupId,searchKey).ToList();
                //if (group.Inventory_SubGroup.Any())
                //{
                //  yield return group;
                //}
                yield return group; 
            }

        }

        public Inventory_Group GetGroupById(int groupId)
        {
          return  _groupRepository.FindOne(x => x.GroupId == groupId);
        }

        public int RateUpdate(DateTime? updateDate,int itemId)
        {
            var updateInde = 0; updateInde=_itemRepository.RateUpdate(updateDate, itemId);
            return updateInde;
        }

        public int MonthlyRateUpdate(DateTime? updateDate)
        {
           return _itemRepository.MonthlyRateUpdate(updateDate);
        }

        private IEnumerable<Inventory_SubGroup> GetAllSubGroups(int groupId,string searchKey)
        {
            var inventorySubGroups =_subGroupRepository.Filter(x => x.GroupId == groupId && x.IsActive);
            foreach (var subGroup in inventorySubGroups)
            {
                subGroup.Inventory_Item = GetAllItems(subGroup.SubGroupId,searchKey).ToList();
                //if (subGroup.Inventory_Item.Any())
                //{
                //    yield return subGroup;
                //}
              yield return subGroup;
            }
        }
        private IEnumerable<Inventory_Item> GetAllItems(int subGroupId,string searchKey)
        {
        //  return _itemRepository.Filter(x => x.IsActive && x.SubGroupId == subGroupId && (x.ItemName.Replace(" ", String.Empty).ToLower().Contains(searchKey.Replace(" ", String.Empty).ToLower()) || searchKey == null));
          return _itemRepository.Filter(x => x.IsActive && x.SubGroupId == subGroupId);
        }
    }
}
