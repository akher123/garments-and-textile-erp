using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class PurchaseTypeRepository : Repository<Inventory_PurchaseType>, IPurchaseTypeRepository
    {
        public PurchaseTypeRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<Inventory_PurchaseType> GetPurchaseTypesByPaging(Inventory_PurchaseType model)
        {
            return
              Context.Inventory_PurchaseType.Where(
                  x => x.IsActive && (x.Title.ToLower().Contains(model.Title) || String.IsNullOrEmpty(model.Title)));
        }
    }
}
