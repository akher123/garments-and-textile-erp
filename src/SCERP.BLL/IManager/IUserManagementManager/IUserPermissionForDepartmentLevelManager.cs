using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common.PermissionModel;
using SCERP.Model;

namespace SCERP.BLL.IManager.IUserManagementManager
{
    public interface IUserPermissionForDepartmentLevelManager
    {
        List<UserPermissionForDepartmentLevel> GetUserPermissionForDepartmentLevel(string userName);
        List<UserCompany> GetUserCompanyList();
        int SaveUserPermissionForDepartmentLevel(List<string> companyBranchUnitDepartment, string userName);
        object GetUserPermissionSector(string userName);
    }
}
