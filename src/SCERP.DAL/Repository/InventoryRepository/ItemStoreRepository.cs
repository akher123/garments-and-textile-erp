using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class ItemStoreRepository : Repository<Inventory_ItemStore>, IItemStoreRepository
    {
        public ItemStoreRepository(SCERPDBContext context)
            : base(context)
        {
        }

  

        public IQueryable<VInventoryItemStore> GetInventoryItemStore(Expression<Func<VInventoryItemStore, bool>> predicate)
        {
            return Context.VInventoryItemStores.Where(predicate);
        }

        public Inventory_ItemStore GetInventoryItemStoreByItemStoreId(long itemStoreId)
        {
            return  Context.Inventory_ItemStore.FirstOrDefault(x => x.ItemStoreId == itemStoreId && x.IsActive);
        }
    }
}
