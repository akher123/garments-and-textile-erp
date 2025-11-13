using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IUserManagementRepository
{
    public interface IUserPermissionForDepartmentLevelRepository : IRepository<UserPermissionForDepartmentLevel>
    {
        List<UserPermissionForDepartmentLevel> GetUserPermissionForDepartmentLevel(string userName);
        int SaveUserPermissionForDepartmentLevel(List<UserPermissionForDepartmentLevel> userPermission);
    }
}
