using System;
using System.Collections.Generic;
using System.Web.Security;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.BLL.Manager;
using SCERP.Model;


namespace SCERP.Web.Provider
{
    public class CustomRoleProvider : RoleProvider
    {
        public Manager Manager { get { return Manager.Instance; } }

        public IUserRoleManager UserRoleManager
        {
            get { return Manager.UserRoleManager; }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //Return all the roles for user.
            //For SCERP we need to return accesslevel also. So the role name might be like this: Leaveapplication-3. 
            // Where Leaveapplication is role name and 3 is AccessLevel.

            var userRole = UserRoleManager.GetUserRole(username).ToArray();
            var userRoleArray = new string[userRole.Length];

            for (var i = 0; i < userRole.Length; i++)
            {
                string userRoleWithAccessLevel = string.Format("{0}-{1}", userRole[i].ModuleFeature.FeatureName.Trim().Replace(" ","").ToLower(), userRole[i].AccessLevel);
                userRoleArray[i] = userRoleWithAccessLevel;
            }
            return userRoleArray;

            //return new string[] { "Leaveapplication-3", "user" };

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (username == "mah")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
