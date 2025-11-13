using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IRquisitiontypeRepository:IRepository<Inventory_RequsitionType>
   {
       IQueryable<Inventory_RequsitionType> GetRquisitiontypesByPaging(Inventory_RequsitionType model);
   }
}
