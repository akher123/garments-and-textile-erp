using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IUserPermissionForEmployeeLevelManager
    {
        List<UserPermissionForEmployeeLevel> GetUserPermissionForEmployeeLevel(string userName);
        object UserPermissionForEmployeeLevelByUserName(string userName);
        int SaveUserPermissionForEmployeeLevelManager(List<int> selectedEmployeeTypes, string userName);
    }
}
