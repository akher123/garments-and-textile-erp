using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<UserRole> GetUserRole(string userName)
        {
             return Context.UserRoles.Where(x => x.UserName == userName && x.IsActive == true).Include(y=>y.ModuleFeature).ToList();
        }

     
    }
}
