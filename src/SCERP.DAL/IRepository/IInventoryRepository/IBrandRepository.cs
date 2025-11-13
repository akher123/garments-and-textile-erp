using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IBrandRepository:IRepository<Inventory_Brand>
   {
       IQueryable<Inventory_Brand> GetBrandsByPaging(Inventory_Brand model);
   }
}
