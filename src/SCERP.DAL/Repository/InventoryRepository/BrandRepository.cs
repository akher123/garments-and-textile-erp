using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class BrandRepository : Repository<Inventory_Brand>, IBrandRepository
    {
        public BrandRepository(SCERPDBContext context) : base(context)
        {

        }
            
        public IQueryable<Inventory_Brand> GetBrandsByPaging(Inventory_Brand model)
        {
            return
                Context.Inventory_Brand.Where(
                    x =>x.IsActive&&( x.Name.ToLower().Contains(model.Name) || String.IsNullOrEmpty(model.Name)));
        }
    }
}
