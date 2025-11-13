using System;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class UserPermissionForDepartmentLevelController : BaseUserManagementController
    {
        //
        // GET: /UserManagement/UserPermissionForDepartmentLevel/
        public ActionResult Index(UserPermissionForDepartmentLevelViewModel model)
        {
            ModelState.Clear();
            var userCompanyList = UserPermissionForDepartmentLevelManager.GetUserCompanyList();
            var allUsers = UserManager.GetAllUsers();
            model.UserCompanies = userCompanyList;
            model.Users = allUsers;
            return View(model);
        }
        public ActionResult Save(UserPermissionForDepartmentLevelViewModel model)
        {
            ModelState.Clear();
            var saveIndex = 0;
            try
            {
               
              saveIndex = UserPermissionForDepartmentLevelManager.SaveUserPermissionForDepartmentLevel(model.CompanyBranchUnitDepartment, model.UserName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return saveIndex > 0 ? ErrorResult("User permission saved successfully") : ErrorResult("Failed to save data");
        }

        public JsonResult UserPermissionForDepartmentLevelByUserName(UserPermissionForDepartmentLevel userPermission)
        {
            
            var userPermissionList =
                UserPermissionForDepartmentLevelManager.GetUserPermissionSector(userPermission.UserName);
            return Json(userPermissionList, JsonRequestBehavior.AllowGet);
        }
	}
}