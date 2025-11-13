
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class BaseUserManagementController : BaseController
    {

        public IModuleManager ModuleManager
        {
            get { return Manager.ModuleManager; }
        }

        public IModuleFeatureManager ModuleFeatureManager
        {
            get { return Manager.ModuleFeatureManager; }
        }

  
        public IUserRoleManager UserRoleManager
        {
            get { return Manager.UserRoleManager; }
        }
        public IUserPermissionForDepartmentLevelManager UserPermissionForDepartmentLevelManager
        {
            get { return Manager.UserPermissionForDepartmentLevelManager; }
        }
        public IUserPermissionForEmployeeLevelManager UserPermissionForEmployeeLevelManager
        {
            get { return Manager.UserPermissionForEmployeeLevelManager; }
        }
   
	}
}