using System;
using System.Linq;
using System.Data.Entity;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class InventoryAuthorizedPersonRepository : Repository<Inventory_AuthorizedPerson>, IInventoryAuthorizedPersonRepository
    {
        public InventoryAuthorizedPersonRepository(SCERPDBContext context) : base(context)
        {

        }

        public bool CheckUserIsStorePerson(int processTypeId, int processId, Guid? userId)
        {
            return
                Context.Inventory_AuthorizedPerson.Any(
                    x => x.ProcessId == processId && x.ProcessTypeId == processTypeId && x.EmployeeId == userId);
        }
    }
}
