using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IInventoryAuthorizedPersonRepository : IRepository<Inventory_AuthorizedPerson>
   {
        bool CheckUserIsStorePerson(int processTypeId, int processId, Guid? userId);
   }
}
