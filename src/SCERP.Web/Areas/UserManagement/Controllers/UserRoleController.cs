using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class UserRoleController : BaseController
    {
        private readonly IModuleFeatureManager moduleFeatureManager;
        private readonly IUserManager userManager;
        private readonly IUserRoleManager userRoleManager;
        private readonly ICompanyManager companyManager;
        public UserRoleController(ICompanyManager companyManager, IModuleFeatureManager moduleFeatureManager, IUserManager userManager, IUserRoleManager userRoleManager)
        {
            this.moduleFeatureManager = moduleFeatureManager;
            this.userManager = userManager;
            this.userRoleManager = userRoleManager;
            this.companyManager = companyManager;
        }
        //
        // GET: /UserManagement/UserRole/
        public ActionResult Index(UserRoleFeatureViewModel model)
        {
            ModelState.Clear();
            var moduleFeatures = moduleFeatureManager.GetModuleFeatures(PortalContext.CurrentUser.CompId, PortalContext.CurrentUser.IsSystemUser, PortalContext.CurrentUser.Name);
            var teTreeViewBuilder = new TreeViewBuilder(moduleFeatures);
            model.ModuleFeatureTreeViews = teTreeViewBuilder.GetModuleFeatureTreeView();
            //model.Users = userManager.GetAllUsers(PortalContext.CurrentUser.CompId);
            model.Companies = companyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(UserRoleFeatureViewModel model)
        {
            var roles = 0;
            if (model.ModuleFeatureId.Count == 0 || model.AccessLevel.Count == 0)
            {
                return ErrorResult();
            }
            var userRoles = ConvertUserRoles(model);
            roles = userRoleManager.SaveUserRoles(userRoles);
            return roles != 0 ? Reload() : ErrorResult();
        }
        [HttpPost]
        public ActionResult CopySave(UserRoleFeatureViewModel model)
        {
            var roles = 0;
            model.UserName = model.NewUserName;
            if (model.ModuleFeatureId.Count == 0 || model.AccessLevel.Count == 0)
            {
                return ErrorResult();
            }
            var userRoles = ConvertUserRoles(model);
            roles = userRoleManager.SaveUserRoles(userRoles);
            return roles != 0 ? Reload() : ErrorResult();
        }


        public ActionResult GetUserRole(string userName)
        {
            var userRoles = userRoleManager.GetUserRole(userName);
            var roles = new List<object>();

            foreach (var userRole in userRoles)
                roles.Add(new { userRole.ModuleFeatureId, userRole.AccessLevel });
            return Json(roles, JsonRequestBehavior.AllowGet);
        }



        private static List<UserRole> ConvertUserRoles(UserRoleFeatureViewModel model)
        {
            var index = 0;
            var userRoles = new List<UserRole>();
            while (index < model.ModuleFeatureId.Count)
            {
                var total = model.AccessLevel.Where(accesslabe => Convert.ToInt32(accesslabe.Split('_')[0]) == model.ModuleFeatureId[index]).Sum(accesslabe => Convert.ToInt32(accesslabe.Split('_')[1]));

                userRoles.Add(total == 1
                    ? new UserRole()
                    {
                        UserName = model.UserName,
                        AccessLevel = 1,
                        ModuleFeatureId = model.ModuleFeatureId[index],
                        CDT = DateTime.Today,
                        CreatedBy =  PortalContext.CurrentUser.UserId,
                        CompId = model.CompId,
                        IsActive = true,
                    }
                    : new UserRole()
                    {
                        UserName = model.UserName,
                        AccessLevel = total == 6 ? 3 : 2,
                        ModuleFeatureId = model.ModuleFeatureId[index],
                        CDT = DateTime.Today,
                        CreatedBy =  PortalContext.CurrentUser.UserId,
                        CompId = model.CompId,
                        IsActive = true
                    });
                index++;
            }
            return userRoles;
        }
    }
}