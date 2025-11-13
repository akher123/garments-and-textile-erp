using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IItemStoreDetailRepository:IRepository<Inventory_ItemStoreDetail>
   {
       List<VItemReceiveDetail> GetVItemReciveDetails(long itemStoreId);
   }
}
