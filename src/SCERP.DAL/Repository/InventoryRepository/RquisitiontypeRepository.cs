using System;
using System.Linq;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class RquisitiontypeRepository : Repository<Inventory_RequsitionType>, IRquisitiontypeRepository
   {
        public RquisitiontypeRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<Inventory_RequsitionType> GetRquisitiontypesByPaging(Inventory_RequsitionType model)
        {
            return
                Context.Inventory_RequsitionType.Where(
                    x => x.IsActive && (x.Title.ToLower().Contains(model.Title) || String.IsNullOrEmpty(model.Title)));
        }
   }
}
