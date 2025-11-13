using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class ItemStoreDetailRepository : Repository<Inventory_ItemStoreDetail>, IItemStoreDetailRepository
   {
        public ItemStoreDetailRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<VItemReceiveDetail> GetVItemReciveDetails(long itemStoreId)
        {
            var itemstoreDetails= Context.VItemReceiveDetails.Where(x=>x.ItemStoreId==itemStoreId).ToList();
            return itemstoreDetails;
        }
   }
}
