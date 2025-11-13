using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface IEmailTemplateManager
    {
        List<EmailTemplate> GetEmailTemplates(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);
        EmailTemplate GetEmailTemplateById(int emailUserId);
        string GetNewRefId();
        int EditEmailTemplate(EmailTemplate modelEmailTemplate);
        int SaveEmailTemplate(EmailTemplate modelEmailTemplate);
        List<EmailTemplate> GetAllEmailTemplates();
    }
}
