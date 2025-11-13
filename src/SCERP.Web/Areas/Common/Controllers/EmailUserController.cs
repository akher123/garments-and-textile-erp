using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class EmailUserController : BaseController
    {
        private readonly IEmailUserManager _emailUserManager;

        public EmailUserController(IEmailUserManager emailUserManager)
        {
            _emailUserManager = emailUserManager;
        }

        public ActionResult Index(EmailUserViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.EmailUsers = _emailUserManager.GetEmailUsers(model.PageIndex, model.sort, model.sortdir,model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult Edit(EmailUserViewModel model)
        {
            ModelState.Clear();
            if (model.EmailUser.EmailUserId>0)
            {
                model.EmailUser = _emailUserManager.GetEmailUserById(model.EmailUser.EmailUserId);
            }
            else
            {
                model.EmailUser.EmailUserRefId = _emailUserManager.GetNewRefId(PortalContext.CurrentUser.CompId);
            }
            return View(model);
        }
        public ActionResult Save(EmailUserViewModel model)
        {
            try
            {
                model.EmailUser.CompId = PortalContext.CurrentUser.CompId;
                var saved = model.EmailUser.EmailUserId > 0 ? _emailUserManager.EditEmailUser(model.EmailUser) : _emailUserManager.SaveEmailUser(model.EmailUser);
                return saved>0 ? Reload() : ErrorResult("Email User Information not save successfully!!");
            
            }
            catch (Exception e)
            {
               Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }
       
        }
	}
}