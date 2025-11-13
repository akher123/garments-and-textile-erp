using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class StoreLedgerRepository :Repository<Inventory_StoreLedger>, IStoreLedgerRepository
    {
        public StoreLedgerRepository(SCERPDBContext context) : base(context)
        {
        }

        public int DeleteRange(IQueryable<Inventory_StoreLedger> inventoryStoreLedgers)
        {
            Context.Inventory_StoreLedger.RemoveRange(inventoryStoreLedgers);
            return  Context.SaveChanges();
           
        }
    }
}
