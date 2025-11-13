using System;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.HRM.Controllers;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class UserPermissionForEmployeeLevelController : BaseUserManagementController
    {
        private IUserManager _userManager;
        public UserPermissionForEmployeeLevelController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public ActionResult Index(UserPermissionForEmployeeLevelViewModel model)
        {
            try
            {
                ModelState.Clear();
                var employeeTypes = EmployeeTypeManager.GetAllEmployeeTypes();
                var allUsers = _userManager.GetAllUsers();
                model.EmployeeTypes = employeeTypes;
                model.Users = allUsers;
            }
            catch (Exception exception)
            { 
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(UserPermissionForEmployeeLevelViewModel model)
        {
            ModelState.Clear();
           
            var saveIndex = 0;
            try
            {
                if (model.SelectedEmployeeTypes.Any()&&!String.IsNullOrEmpty(model.UserName))
                {
                    saveIndex = UserPermissionForEmployeeLevelManager.SaveUserPermissionForEmployeeLevelManager(model.SelectedEmployeeTypes, model.UserName);
                }
                else
                {
                    return ErrorResult("Select Employee type ");
                }
              
            }
            catch (Exception exception)
            {
                return ErrorResult(exception.Message);
            }
       
            return saveIndex > 0 ? ErrorResult("User permission saved successfully") : ErrorResult("Failed to save data");
        }
        public JsonResult UserPermissionForEmployeeLevelByUserName(UserPermissionForEmployeeLevel userPermission)
        {
          
            var userPermissionList =
                UserPermissionForEmployeeLevelManager.UserPermissionForEmployeeLevelByUserName(userPermission.UserName);
            return Json(userPermissionList, JsonRequestBehavior.AllowGet);
        }  
	}
}