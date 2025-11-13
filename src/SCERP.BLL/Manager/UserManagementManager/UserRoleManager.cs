using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.DAL.Repository.UserManagementRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.UserManagementManager
{
    public class UserRoleManager : BaseManager, IUserRoleManager
    {
        readonly List<UserRole> _newUserRoles = new List<UserRole>();
        public IUserRoleRepository UserRoleRepository { get; set; }

        public UserRoleManager(SCERPDBContext context)
        {
            UserRoleRepository = new UserRoleRepository(context);
        }

        public List<UserRole> GetUserRole(string userName)
        {
            return UserRoleRepository.GetUserRole(userName);
        }
        public int SaveUserRoles(List<UserRole> userRoles)
        {
            var effectIndex = 0;
            foreach (var userRole in userRoles)
            {
                _newUserRoles.Add(userRole);
            } 

            try
            {
                var existsuserRole = UserRoleRepository.GetUserRole(userRoles[0].UserName);

                if (existsuserRole == null)
                {
                    return UserRoleRepository.SaveList(userRoles);
                }

                foreach (var userRole in existsuserRole)
                {
                    if (userRoles.Exists(x => x.UserName == userRole.UserName && x.ModuleFeatureId == userRole.ModuleFeatureId && x.AccessLevel == userRole.AccessLevel))
                    {
                        _newUserRoles.RemoveAll(x => x.UserName == userRole.UserName && x.ModuleFeatureId == userRole.ModuleFeatureId && x.AccessLevel == userRole.AccessLevel);
                    }
                    else
                    {
                        var role = userRole;
                        effectIndex = UserRoleRepository.Delete(x => x.UserName == role.UserName && x.ModuleFeatureId == role.ModuleFeatureId);
                    }
                }

                effectIndex = _newUserRoles.Count > 0 ? UserRoleRepository.SaveList(_newUserRoles) : 1;
            }
            catch (Exception)
            {
                effectIndex = 0;
            }
            return effectIndex;
        }
    }
}
