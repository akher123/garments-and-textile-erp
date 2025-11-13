using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.UserManagement.Controllers
{
    public class UserMerchandiserController : BaseController
    {
        private readonly IUserMerchandiserManager _userMerchandiserManager;
        private readonly IUserManager _userManager;
        private readonly IMerchandiserManager _merchandiserManager;
        public UserMerchandiserController(IMerchandiserManager merchandiserManager,IUserManager userManager,IUserMerchandiserManager userMerchandiserManager)
        {
            this._userMerchandiserManager = userMerchandiserManager;
            this._userManager = userManager;
            this._merchandiserManager = merchandiserManager;
        }
        public ActionResult Index(UserMerchandiserViewModel model)
        {
            var allUsers = _userManager.GetAllUsers();
            model.Merchandisers= _merchandiserManager.GetMerchandisers();
            model.Users = allUsers;
            return View(model);
        }
        public JsonResult GetPermitedUserMerchandiser(UserMerchandiserViewModel model)
        {

            var userMerchandiserList = _userMerchandiserManager.GetPermitedUserMerchandiser(model.EmployeeId);
            return Json(userMerchandiserList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(UserMerchandiserViewModel model)
        {
            ModelState.Clear();
            string message = "";
            var saveIndex = 0;
            try
            {
                if (model.EmployeeId!=default(Guid))
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