using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class EmailTemplateUserViewModel : ProSearchModel<EmailTemplateUserViewModel>
    {
        public List<EmailTemplateUser> EmailTemplateUsers { get; set; }
        public List<EmailUser> EmailUsers { get; set; }
        public List<EmailTemplate> EmailTemplates { get; set; }
        public EmailTemplateUser TemplateUser { get; set; }

        public EmailTemplateUserViewModel()
        {
            this.EmailTemplates=new List<EmailTemplate>();
            this.EmailUsers=new List<EmailUser>();
            this.TemplateUser=new EmailTemplateUser();
            this.EmailTemplateUsers = new List<EmailTemplateUser>();

        }
        public IEnumerable<SelectListItem> EmailUserSelectListItem
        {
            get { return new SelectList(EmailUsers, "EmailUserId", "EmailUserName"); }
        }
        public IEnumerable<SelectListItem>EmailTempalteSelectListItem
        {
            get { return new SelectList(EmailTemplates, "EmailTemplateId", "EmailTemplateName"); }
        }
        public IEnumerable<SelectListItem> STypeSelectListItem
        {
            get { return new SelectList(new[] {"TO" ,"CC" }, "SendingType"); }
        }
    }
}