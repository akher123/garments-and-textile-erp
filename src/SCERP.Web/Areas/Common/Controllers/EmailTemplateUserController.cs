using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class EmailTemplateUserController : BaseController
    {
        private readonly IEmailTemplateUserManager _emailTemplateUserManager;
        private readonly IEmailUserManager _emailUserManager;
        private readonly IEmailTemplateManager _emailTemplateManager;
        public EmailTemplateUserController(IEmailTemplateUserManager emailTemplateUserManager, IEmailTemplateManager emailTemplateManager, IEmailUserManager emailUserManager)
        {
            _emailTemplateUserManager = emailTemplateUserManager;
            _emailTemplateManager = emailTemplateManager;
            _emailUserManager = emailUserManager;
        }

        public ActionResult Index(EmailTemplateUserViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.EmailTemplateUsers = _emailTemplateUserManager.GetEmailTemplateUserUsers(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        public ActionResult Edit(EmailTemplateUserViewModel model)
        {
            ModelState.Clear();
            if (model.TemplateUser.EmailTamplateUserId > 0)
            {
                model.TemplateUser = _emailTemplateUserManager.GetEmailTemplateUseById(model.TemplateUser.EmailTamplateUserId);
            }
            model.EmailTemplates = _emailTemplateManager.GetAllEmailTemplates();
            model.EmailUsers = _emailUserManager.GetAllEmailUsers(PortalContext.CurrentUser.CompId);
            return View(model);
        }
        public ActionResult Save(EmailTemplateUserViewModel model)
        {
            try
            {
                model.TemplateUser.CompId = PortalContext.CurrentUser.CompId;
                var saved = model.TemplateUser.EmailTamplateUserId > 0 ? _emailTemplateUserManager.EditEmailTemplateUser(model.TemplateUser) : _emailTemplateUserManager.SaveEmailTamplateUser(model.TemplateUser);
                return saved > 0 ? Reload() : ErrorResult("Email Tempalte User Information not save successfully!!");

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }


	}
}