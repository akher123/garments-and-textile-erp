using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class UserTnaResponsibleController:BaseController
    {

       private readonly IUserMerchandiserManager _userMerchandiserManager;
        private readonly IUserManager _userManager;
        private readonly ITimeAndActionManager _timeAndActionManager;
        public UserTnaResponsibleController( IUserManager userManager, IUserMerchandiserManager userMerchandiserManager, ITimeAndActionManager timeAndActionManager)
        {
            this._userMerchandiserManager = userMerchandiserManager;
            _timeAndActionManager = timeAndActionManager;
            this._userManager = userManager;
   
        }

        public ActionResult Index(UserTnaResponsibleViewModel model)
        {
            var allUsers = _userManager.GetAllUsers();
            model.Responsibles = _timeAndActionManager.GetTnaRespobslibles(PortalContext.CurrentUser.CompId);
            model.Users = allUsers;
            return View(model);
        }
        public JsonResult GetPermitedUserMerchandiser(UserTnaResponsibleViewModel model)
        {

            var userMerchandiserList = _userMerchandiserManager.GetPermitedUserMerchandiser(model.EmployeeId);
            return Json(userMerchandiserList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(UserTnaResponsibleViewModel model)
        {
            ModelState.Clear();
            string message = "";
            var saveIndex = 0;
            try
            {
                if (model.EmployeeId != default(Guid))
                {
                    saveIndex = _userMerchandiserManager.SaveUserUserMerchandiser(model.MerchandiserIdList, model.EmployeeId);
                }
                else
                {
                    return ErrorResult("Select Merchandiser ");
                }

            }
            catch (Exception exception)
            {
                return ErrorResult(exception.Message);
                message = exception.Message;
            }

            return saveIndex > 0 ? ErrorResult("User permission saved successfully") : ErrorResult(message);
        }
    }
}