using System;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using System.Web;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManager _userManager;
        private IEmployeeManager _employeeManager;
        public HomeController(IUserManager userManager, IEmployeeManager employeeManager)
        {
            this._userManager = userManager;
            _employeeManager = employeeManager;
        }

        public ActionResult Index()
        {
            var existingCookie = Request.Cookies["userCookie"];
            if (existingCookie != null)
            {
                string userName = Request.Cookies["userCookie"]["userName"];
                string password = Request.Cookies["userCookie"]["password"];
                if (!PortalContext.CurrentUser.Validated)
                    Membership.ValidateUser(userName, password);

            }
            if (PortalContext.CurrentUser.Validated)
            {
                string imageUrl = _employeeManager.GetEmployeeImageUrlById(PortalContext.CurrentUser.UserId);
                ViewBag.ImageUrl = imageUrl;

                return View();
            }
            return RedirectToAction("LogIn", "Home");
        }

        public ActionResult Login(string username, string returnUrl)
        {
            var existingCookie = Request.Cookies["userCookie"];
            if (existingCookie != null)
            {
                string userName = Request.Cookies["userCookie"]["userName"];
                ViewBag.UserName = userName;

                string password = Request.Cookies["userCookie"]["password"];
                ViewBag.Password = password;
            }
            else
            {
                ViewBag.UserName = "";
                ViewBag.Password = "";
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string userName, string password, string checkboxid)
        {
            var success = false;
            var request = HttpContext.Request;
            if (!String.IsNullOrWhiteSpace(userName) && !String.IsNullOrWhiteSpace(password))
            {

                success = Membership.ValidateUser(userName, password);

                if (!success)
                {
                    ViewBag.Message = "Enter Currect User And Password";
                    return View();
                }
                if (!string.IsNullOrEmpty(checkboxid))
                {
                    Response.Cookies["userCookie"]["userName"] = userName;
                    Response.Cookies["userCookie"]["password"] = password;
                    Response.Cookies["userCookie"].Expires = DateTime.Now.AddMonths(1);
                    FormsAuthentication.RedirectFromLoginPage(userName, true); //Very important. If not use, layout functionality does not work
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(userName, false);
                }
                if (AppConfig.IsEnableUserLogTime)
                {

                    UserLogTime userLogTime = new UserLogTime();
                    if (request.Browser != null)
                    {

                        userLogTime.BrowserName = request.Browser.Browser;
                        userLogTime.BrowserVerssion = request.Browser.Version;
                    }
                    userLogTime.LoginTime = DateTime.Now;
                    userLogTime.UserHostAddress = request.UserHostAddress;
                    userLogTime.Offline = true;
                    userLogTime.SessionId = Session.SessionID;
                    success = _userManager.SaveUserLogTime(userLogTime);
                }

            }
            if (success)
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            else
            {
                ViewBag.Message = "Enter Currect User And Password";
                return View();
            }
           

        }

        public ActionResult EmployeeDetails(int id)
        {
            ViewBag.EmployeeId = id;
            return View("EmployeeDetail");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            if (AppConfig.IsEnableUserLogTime)
            {
              _userManager.UpdateUserLogTime(Session.SessionID);
            }
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userId, string email)
        {
            try
            {
                string userID = "";
                string emailID = "";
                if (String.IsNullOrWhiteSpace(userId) && String.IsNullOrWhiteSpace(email))
                {
                    ViewBag.Message = "Enter User Id or Email";
                }
                else
                {
                    bool isUserIdExist = _userManager.IsUserIdExist(userId);
                    if (isUserIdExist)
                    {
                        userID = userId;
                        emailID = _userManager.GetEmailByUserId(userId);
                    }
                    else
                    {
                        bool isEmailExist = _userManager.IsEmailExist(email);
                        if (isEmailExist)
                        {
                            emailID = email;
                            userID = _userManager.GetUserIdByEmail(email);
                        }
                        else
                        {
                            ViewBag.Message = "User Id or email does not exist !";
                        }
                    }
                }
                bool isSent = _userManager.SendForgotPassword(userID, emailID);
                if (isSent)
                {
                    ViewBag.Message = "Password sent your email successfully";
                }
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Failed to send password to email";
                Errorlog.WriteLog(exception);
            }
            return View("ForgotPassword");
        }

        public ActionResult RefreshWindow()
        {
            return Content(string.Empty);
        }

        [HttpPost]
        public JsonResult IsSessionAlive()
        {
            if (HttpContext.Session.Contents.Count == 0)
            {
                return this.Json(new { IsAlive = false }, JsonRequestBehavior.AllowGet);
            }
            return this.Json(new { IsAlive = true }, JsonRequestBehavior.AllowGet);
        }

        public FileResult ScerpApp()
        {

            string path = Server.MapPath("~/Content/Document/soft-codeapp04Feb2017.apk");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = "ScerpApp.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult GetResult(string key)
        {
            object data = _userManager.GetData(key);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}
