using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class EmailTemplateViewModel : ProSearchModel<EmailTemplateViewModel>
    {
        public List<EmailTemplate> EmailTemplates { get; set; }
        public EmailTemplate EmailTemplate { get; set; }

        public EmailTemplateViewModel()
        {
            this.EmailTemplates=new List<EmailTemplate>();
            this.EmailTemplate=new EmailTemplate();
        }
    }
}