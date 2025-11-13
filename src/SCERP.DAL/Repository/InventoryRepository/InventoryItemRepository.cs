using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class InventoryItemRepository : Repository<Inventory_Item>, IInventoryItemRepository
    {
        public InventoryItemRepository(SCERPDBContext context) : base(context)
        {
        }

 

        public Inventory_Item GetItemById(int itemId, int branchId)
        {
            return
                Context.Inventory_Item.Include(x => x.Inventory_SubGroup.Inventory_Group)
                    .Where(x => x.IsActive)
                    .FirstOrDefault(x => x.ItemId == itemId);
        }

        public bool CheckAnyItemExist(int itemId, int branchId)
        {
            return Context.Inventory_Item.Include(x => x.Inventory_SubGroup.Inventory_Group)
                    .Any(x => x.IsActive );

        }
        public bool IsBranchWiseItemExist(InventorySearchField searchField)
        {
            return Context.Inventory_Item.Include(x => x.Inventory_SubGroup.Inventory_Group)
                     .Any(x => x.IsActive );
        }


        public List<Inventory_Item> AutocompliteItemByBranch(string itemName)
        {
            var items = Context.Inventory_Item.Where( x => (x.ItemName.Replace(" ", String.Empty).ToLower().Contains(itemName.Replace(" ", String.Empty).ToLower()) || x.ItemCode.Replace(" ", String.Empty).ToLower() .Contains(itemName.Replace(" ", String.Empty).ToLower()))).Take(50);
            return items.ToList();

        }

        public string GetItemCode(int? subGroupId, int? branchId)
        {
            var maxItemCode =
                Context.Database.SqlQuery<string>("select RIGHT('00000000'+ CONVERT(VARCHAR(8),isnull(max(Item.ItemCode),00)+1),8) " + "as ItemCode " +
                                                  "from Inventory_Item as Item " +
                                                  "where Item.IsActive=1 and Item.SubGroupId=" + subGroupId +
                                                   "").SingleOrDefault();
            return maxItemCode;
        }

        public string GetMaxSubGroupCode(int groupId)
        {
              var maxSubGroupCode = Context.Database.SqlQuery<string>(
                "select RIGHT('00000'+ CONVERT(VARCHAR(5),isnull(max(SubGroup.SubGroupCode),00)+1),5)  as SubGroupCode " +
                "from Inventory_SubGroup as SubGroup " +
                " where SubGroup.IsActive=1 and  SubGroup.GroupId=" + groupId + "").SingleOrDefault();
            
            return maxSubGroupCode;
        }

        public int RateUpdate(DateTime? updateDate,int itemId)
        {
            return Context.Database.ExecuteSqlCommand("exec SpItemRateUpdate {0},{1}",updateDate,itemId);
        }

        public int MonthlyRateUpdate(DateTime? updateDate){
            return Context.Database.ExecuteSqlCommand("exec [dbo].[spMonthlyRateUpdate] {0}", updateDate);
        }
    }
}
