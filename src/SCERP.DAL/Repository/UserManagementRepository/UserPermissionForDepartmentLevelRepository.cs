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
    public class UserPermissionForDepartmentLevelRepository : Repository<UserPermissionForDepartmentLevel>, IUserPermissionForDepartmentLevelRepository
    {
        public UserPermissionForDepartmentLevelRepository(SCERPDBContext context)
            : base(context)
        {

        }

        public List<UserPermissionForDepartmentLevel> GetUserPermissionForDepartmentLevel(string userName)
        {
            try
            {

                return Context.UserPermissionForDepartmentLevels.Where(x => x.UserName == userName && x.IsActive == true)
                    .Include(y => y.User)
                    .Include(y => y.Company)
                    .Include(y => y.Branch)
                    .Include(y => y.BranchUnit)
                    .Include(y => y.BranchUnitDepartment)
                    .Include(y => y.BranchUnit.Unit)
                    .Include(y => y.BranchUnitDepartment.UnitDepartment.Department)
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public int SaveUserPermissionForDepartmentLevel(List<UserPermissionForDepartmentLevel>userPermission )
        {
            int saveIndex = 0;
            try
            {
               Context.UserPermissionForDepartmentLevels.AddRange(userPermission);
                saveIndex = Context.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
            return saveIndex;
        }
    }
}
