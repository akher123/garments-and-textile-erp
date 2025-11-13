using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class SizeRepository : Repository<Inventory_Size>, ISizeRepository
    {
        public SizeRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<Inventory_Size> GetSizeListByPaging(Inventory_Size model)
        {
            return
                Context.Inventory_Size.Where(
                    x =>x.IsActive &&( x.Title.ToLower().Contains(model.Title) || String.IsNullOrEmpty(model.Title)));
        }
    }
}
