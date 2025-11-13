using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IUserRoleManager
    {
        List<UserRole> GetUserRole(string userName);
        int SaveUserRoles(List<UserRole> userRoles);
    }
}
