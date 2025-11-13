using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class UserPermissionForEmployeeLevelRepository : Repository<UserPermissionForEmployeeLevel>, IUserPermissionForEmployeeLevelRepository
    {
        public UserPermissionForEmployeeLevelRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<UserPermissionForEmployeeLevel> GetUserPermissionForEmployeeLevel(string userName)
        {
            try
            {

                return Context.UserPermissionForEmployeeLevels.Where(x => x.UserName == userName && x.IsActive == true)
                    .Include(y => y.User)
                    .Include(y => y.EmployeeType)
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }
    }
}
