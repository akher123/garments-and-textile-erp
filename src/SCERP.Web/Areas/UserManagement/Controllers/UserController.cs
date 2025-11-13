using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.BLL.IManager.IUserManagementManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.UserManagement.Models.ViewModels;

namespace SCERP.Web.Areas.UserManagement.Controllers
{


    public class UserController : BaseUserManagementController
    {
        private readonly IUserManager _userManager;
        private readonly ICompanyManager companyManager;
        public UserController(ICompanyManager companyManager, IUserManager userManager)
        {
            _userManager = userManager;
            this.companyManager = companyManager;
        }
        private readonly int _pageSize = AppConfig.PageSize;

        //[AjaxAuthorize(Roles = "user-3")]
        public ActionResult Index(int? page, UserViewModel model)
        {
           
            int totalRecords = 0;
            model.Users = _userManager.GetUsersByPaging(model.PageIndex, model.sort, model.sortdir, model.SearchByUser, out totalRecords) ?? new List<User>();
            model.TotalRecords = totalRecords;
            return View(model);

        }

        //[AjaxAuthorize(Roles = "user-3")]
        public ActionResult Edit(UserViewModel model)
        {
            ModelState.Clear();

            try
            {
                if (model != null && model.Id > 0)
                {
                    var user = _userManager.GetUserById(model.Id);
                    var employeeCardId = user.Employee.EmployeeCardId;
                    model.Employee = new Employee { EmployeeCardId = employeeCardId };
                    model.UserName = user.UserName;
                    model.DomainName = user.DomainName;
                    model.TnaResponsible = user.TnaResponsible;
                    model.Password = "12345";
                    model.ConfirmPassword = "12345";
                    model.EmailAddress = user.EmailAddress;
                    model.Contact = user.Contact;
                    ViewBag.Title = "Edit User";
                    ViewBag.TnaResponsibles = new SelectList(new Dropdown[] 
                    {   new Dropdown { Id = "ALL", Value = "ALL" },
                        new Dropdown { Id = "Dyeing", Value = "Dyeing" },
                        new Dropdown { Id = "IE", Value = "IE" },
                        new Dropdown { Id = "Knitting", Value = "Knitting" }, 
                        new Dropdown { Id = "Merchant", Value = "Merchant" }, 
                        new Dropdown { Id = "Planning", Value = "Planning" },
                        new Dropdown { Id = "Production", Value = "Production" },
                        new Dropdown { Id = "Store", Value = "Store" } }, "Id", "Value",model.TnaResponsible);
                }
                else
                {
                    model.DomainName = PortalContext.CurrentUser.DomainName;
                    ViewBag.Title = "Add User";

                }
                model.Companies = companyManager.GetAllCompanies(PortalContext.CurrentUser.CompId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return View(model);
        }


        //[AjaxAuthorize(Roles = "user-3")]
        public ActionResult Save(UserViewModel model)
        {
            var affectedValue = 0;
            try
            {
                var employee = EmployeeManager.GetEmployeeByCardId(model.Employee.EmployeeCardId);
                if (employee != null)
                {

                    model.Contact.EmployeeId = employee.EmployeeId;
                    User user = new User
                    {
                        Id = model.Id,
                        EmployeeId = employee.EmployeeId,
                        EmailAddress = model.Contact.Email,
                        UserName = model.UserName + model.DomainName,
                        Password = model.Password,
                        DomainName = model.DomainName,
                        Contact = model.Contact,
                        TnaResponsible =model.TnaResponsible
                    };
                    bool isExist = _userManager.IsUserExist(user);
                    if (isExist)
                    {
                        return ErrorResult("User Name:" + model.UserName + " " + "Already Exist ! Please Entry another one");
                    }
                    else
                    {
                        if (model.Id > 0)
                        {
                            affectedValue= _userManager.EditUser(user);
                            return Reload();
                        }
                        else
                        {
                            affectedValue= _userManager.SaveUser(user);
                            bool isSent = _userManager.SendForgotPassword(user.UserName, user.EmailAddress);
                            if (isSent)
                            {
                                ViewBag.Message = "Password sent your email successfully";
                            }
                        }
                   
                    }

                }
                else
                {
                    return Json(new { Success = false, Message = "Employee doesn't exist", Error = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return affectedValue > 0 ? Reload() : ErrorMessageResult();
        }


        //[AjaxAuthorize(Roles = "user-3")]
        public ActionResult Delete(int id)
        {
            var deleted = 0;
            var user = _userManager.GetUserById(id) ?? new User();
            deleted = _userManager.DeleteUser(user);
            return (deleted > 0) ?  ErrorResult("Inactive user Sucessfully") : ErrorResult("Failed to Inactive User");
        }

        [HttpGet]
        public ActionResult PassWordChange(UserViewModel model)
        {
            ModelState.Clear();
            model.OldPassword = String.Empty;
            model.UserName = PortalContext.CurrentUser.Name;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeCurrentPassword(UserViewModel model)
        {
            if (!String.IsNullOrWhiteSpace(model.OldPassword) && !String.IsNullOrWhiteSpace(model.Password) && !String.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                if (model.Password == model.ConfirmPassword)
                {
                    ModelState.Clear();
                    int index = _userManager.ChangeCurrentPassword(model.OldPassword, model);
                    if (index > 0)
                    {
                        return ErrorResult("Reset password Sucessfully.");
                    }
                }
                else
                {
                    return ErrorResult("Password doesn't match!");
                }
            }
            return View(model);
        }

        public JsonResult GetDomainByCompany(string compId)
        {

            string domainName = companyManager.GetCompanyDomaun(compId);

            return Json(domainName, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsersByCompany(string compId)
        {
            var users = _userManager.GetUsersByCompany(compId,PortalContext.CurrentUser.IsSystemUser,PortalContext.CurrentUser.Name);
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActiveUsers(string empName)
        {
            List<User> users = _userManager.GetActiveUsers(empName);
            var usersImages = users.Select(x => new
            {
                icon = Url.Content(x.Employee.PhotographPath),
                label = x.Employee.Name,
                value = x.EmployeeId

            });
            return Json(usersImages, JsonRequestBehavior.AllowGet);
        }
  

        
    }

}