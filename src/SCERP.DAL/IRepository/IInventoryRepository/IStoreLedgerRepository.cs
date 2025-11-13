using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IStoreLedgerRepository:IRepository<Inventory_StoreLedger>
    {
        int DeleteRange(IQueryable<Inventory_StoreLedger> inventoryStoreLedgers);
    }
}
