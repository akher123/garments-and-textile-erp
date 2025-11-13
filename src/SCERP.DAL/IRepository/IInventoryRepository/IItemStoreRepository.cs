using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IItemStoreRepository:IRepository<Inventory_ItemStore>
    {
        IQueryable<VInventoryItemStore> GetInventoryItemStore(Expression<Func<VInventoryItemStore, bool>> predicate);
        Inventory_ItemStore GetInventoryItemStoreByItemStoreId(long itemStoreId);
    }
}
