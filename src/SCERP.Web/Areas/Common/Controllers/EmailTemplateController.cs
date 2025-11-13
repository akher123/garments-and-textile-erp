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
    public class EmailTemplateController : BaseController
    {
        private IEmailTemplateManager _emailTemplateManager;

        public EmailTemplateController(IEmailTemplateManager emailTemplateManager)
        {
            _emailTemplateManager = emailTemplateManager;
        }

        public ActionResult Index(EmailTemplateViewModel model)
        {
            ModelState.Clear();
            int totalRecords = 0;
            model.EmailTemplates = _emailTemplateManager.GetEmailTemplates(model.PageIndex, model.sort, model.sortdir, model.SearchString, out totalRecords);
            return View(model);
        }
        public ActionResult Edit(EmailTemplateViewModel model)
        {
            ModelState.Clear();
            if (model.EmailTemplate.EmailTemplateId > 0)
            {
                model.EmailTemplate = _emailTemplateManager.GetEmailTemplateById(model.EmailTemplate.EmailTemplateId);
            }
            else
            {
                model.EmailTemplate.EmailTemplateRefId = _emailTemplateManager.GetNewRefId();
            }
            return View(model);
        }
        public ActionResult Save(EmailTemplateViewModel model)
        {
            try
            {
         
                var saved = model.EmailTemplate.EmailTemplateId > 0 ? _emailTemplateManager.EditEmailTemplate(model.EmailTemplate) : _emailTemplateManager.SaveEmailTemplate(model.EmailTemplate);
                return saved > 0 ? Reload() : ErrorResult("Email Template Information not save successfully!!");

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }

        }
    }
}