using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IPurchaseTypeRepository : IRepository<Inventory_PurchaseType>
    {
        IQueryable<Inventory_PurchaseType> GetPurchaseTypesByPaging(Inventory_PurchaseType model);
    }
}
