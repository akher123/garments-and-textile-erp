using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IInventorySubGroupRepository:IRepository<Inventory_SubGroup>
   {
       List<Inventory_SubGroup> AutocompliteSubGroup(string subGroupName, int groupId, int branchId);
   }
}
